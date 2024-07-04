// Copyright (C) 2009 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Moonlight.SecurityModel;

class Program {

	class RevisionComparer : IComparer<int> {

		// more recent revision on top
		public int Compare (int x, int y)
		{
			return y - x;
		}
	}

	public class AuditData {
		public string CurrentHash;
		public SortedDictionary<int,string> Comments;
		public bool Exists;

		public AuditData ()
		{
			CurrentHash = String.Empty;
			Comments = new SortedDictionary<int, string> (new RevisionComparer ());
			Exists = false;
		}
	}

	static SortedDictionary<string, AuditData> Data = new SortedDictionary<string, AuditData> (StringComparer.InvariantCulture);

	static void ReadFile (string filename)
	{
		if (!File.Exists (filename)) {
			Console.WriteLine ("File '{0}' does not exists, starting from scratch!", filename);
			return;
		}

		int line_no = 0;
		AuditData data = null;
		using (StreamReader sr = new StreamReader (filename)) {
			while (!sr.EndOfStream) {
				string line = sr.ReadLine ();
				line_no++;

				if (line.Length == 0)
					continue;

				if (Char.IsWhiteSpace (line [0])) {
					int s = line.IndexOf ("\tr") + 2;
					// newer entries uses dates, e.g. 20100817, instead of SVN revision number
					// since GIT digests are not usuable for sorting
					if (s == -1) {
						s = line.IndexOf ('\t') + 1;
					}
					int e = line.IndexOf ('\t', s);
					int revision = -1;

					try {
						if (Int32.TryParse (line.Substring (s, e - s), out revision)) {
							string comments = line.Substring (e).TrimStart ();
							data.Comments.Add (revision, comments);
						}
					}
					catch {
						Console.WriteLine ("{0}: error line #{1}", filename, line_no);
						throw;
					}
				} else {
					if (line [0] == '!') {
						data = new AuditData ();
						data.CurrentHash = line.Trim ();
					} else {
						Data.Add (line.Trim (), data);
					}
				}
			}
		}
	}

	static void Add (MethodDefinition method, string hash, int revision)
	{
		AuditData data;
		string name = method.GetFullName ();

		if (Data.TryGetValue (name, out data)) {
			if (hash != data.CurrentHash) {
				// code has changed, a new review is required
				data.CurrentHash = hash;
				string message = "unaudited - modified";
				if (method.IsVisible ())
					message += " - VISIBLE";
				if (method.IsInternalCall)
					message += " - ICALL";
				try {
					data.Comments.Add (revision, message);
				}
				catch (ArgumentException) {
					Console.WriteLine ("Comments for revision #{0} already exist on '{1}'.", revision, name);
					Environment.Exit (1);
				}
			}
			data.Exists = true;
		} else {
			// new code - review is needed
			data = new AuditData ();
			data.CurrentHash = hash;
			string message = "unaudited - new";
			if (method.IsVisible ())
				message += " - VISIBLE";
			if (method.IsInternalCall)
				message += " - ICALL";
			data.Comments.Add (revision, message);
			Data.Add (name, data);
			data.Exists = true;
		}
	}

	static MD5 hash = MD5.Create ();

	static int GetOffset (Instruction ins)
	{
		if (ins == null)
			return -1;
		return ins.Offset;
	}

	// a bit resource heavy but it's not really time critical
	static string GetHash (MethodDefinition method)
	{
		bool log = (debug_log_enabled && (method.ToString () == debug_log_method));
		byte [] digest = null;
		using (MemoryStream ms = new MemoryStream ()) {
			using (StreamWriter sw = new StreamWriter (ms)) {
				// note: we could hash more (e.g. attributes) but that's unlikely to require a new review
				sw.WriteLine (method.GetFullName ());
				sw.WriteLine (method.Attributes);
				sw.WriteLine (method.ImplAttributes);
				if (log) {
					Console.WriteLine (method.GetFullName ());
					Console.WriteLine (method.Attributes);
					Console.WriteLine (method.ImplAttributes);
				}
				// an icall (or pinvoke) can be audited but won't have any IL
				if (method.HasBody)
					HashBody (method.Body, sw, log);
				sw.Flush ();
				ms.Position = 0;
				hash.Initialize ();
				digest = hash.ComputeHash (ms);
			}
		}
		StringBuilder sb = new StringBuilder ("!SSC-", 37);
		foreach (byte b in digest) {
			sb.Append (b.ToString ("X2"));
		}
		return sb.ToString ();
	}

	static void HashBody (MethodBody body, StreamWriter sw, bool log)
	{
		foreach (Instruction instr in body.Instructions) {
			sw.WriteLine ("{0}#{1}#{2}", instr.Offset, instr.OpCode.Value, FormatOperand (instr, sw, log));
			if (log)
				Console.WriteLine ("{0}#{1}#{2}", instr.Offset, instr.OpCode.Value, FormatOperand (instr, sw, log));
		}

		if (!body.HasExceptionHandlers)
			return;

		List<string> handlers = new List<string> ();
		foreach (ExceptionHandler eh in body.ExceptionHandlers) {
			handlers.Add (String.Format ("{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}", eh.HandlerType, eh.CatchType,
				GetOffset (eh.TryStart), GetOffset (eh.TryEnd), 
				GetOffset (eh.HandlerStart), GetOffset (eh.HandlerEnd),
				GetOffset (eh.FilterStart), GetOffset (eh.FilterEnd)));
		}
		// we must preserve order else the hash will be different for the same handlers
		if (handlers.Count > 1)
			handlers.Sort (StringComparer.InvariantCulture);
		foreach (string s in handlers) {
			sw.WriteLine (s);
			if (log)
				Console.WriteLine (s);
		}
	}

	static string FormatOperand (Instruction instr, StreamWriter sw, bool log)
	{
		switch (instr.OpCode.OperandType) {
		case OperandType.InlineNone:
			return string.Empty;
		case OperandType.InlineBrTarget:
		case OperandType.ShortInlineBrTarget:
			return ((Instruction) instr.Operand).Offset.ToString ();
		case OperandType.InlineSwitch:
			return ((Instruction []) instr.Operand).Aggregate (" ", (s, i) => s + i.Offset.ToString ());
		case OperandType.InlineField:
		case OperandType.InlineType:
		case OperandType.InlineSig:
			return ((IMetadataTokenProvider) instr.Operand).ToString ();
		case OperandType.InlineMethod:
			Code code = instr.OpCode.Code;
			if ((code == Code.Ldftn) || (code == Code.Ldvirtftn)) {
				// anonymous methods can change name between compilation, so we include their content in the loader
				MethodDefinition md = (instr.Operand as MethodDefinition);
				if (md != null && md.HasBody && md.Name.StartsWith ("<")) {
					HashBody (md.Body, sw, log);
					return String.Empty;
				}
			}
			return ((IMetadataTokenProvider) instr.Operand).ToString ();
		case OperandType.InlineTok:
			string s = instr.Operand.ToString ();
			// other details will change between compilations, generating a new digest each time
			if (s.StartsWith ("<PrivateImplementationDetails>"))
				return "<PrivateImplementationDetails>";
			return s;
		case OperandType.ShortInlineVar:
		case OperandType.InlineVar:
			return ((VariableDefinition) instr.Operand).Index.ToString ();
		case OperandType.ShortInlineArg:
		case OperandType.InlineArg:
			return (((ParameterDefinition) instr.Operand).Index + 1).ToString ();
		case OperandType.ShortInlineI:
		case OperandType.ShortInlineR:
		case OperandType.InlineI:
		case OperandType.InlineI8:
		case OperandType.InlineR:
			return ((IFormattable) instr.Operand).ToString (null, CultureInfo.InvariantCulture);
		case OperandType.InlineString:
			return (string) instr.Operand;
		default:
			throw new NotImplementedException ();
		}
	}

	static bool NeedReview (MethodDefinition method)
	{
		// report all icalls that are not SC (visible or not)
		if (method.IsInternalCall)
			return !method.IsSecurityCritical ();
		// is it SSC ?
		return method.IsSecuritySafeCritical ();
	}

	static void ProcessMethod (MethodDefinition method, int revision)
	{
		if (!NeedReview (method)) {
			// we check the type too, since we want to review all methods of a SSC type
			TypeDefinition type = (method.DeclaringType as TypeDefinition);
			if (!type.IsSecuritySafeCritical ())
				return;
		}

		try {
			Add (method, GetHash (method), revision);
		}
		catch (Exception e) {
			Console.WriteLine ("Error on method '{0}': {1}", method, e);
			Environment.Exit (1);
		}
	}

	static void ProcessAssembly (string assemblyName, int revision)
	{
		AssemblyDefinition ad = AssemblyDefinition.ReadAssembly (assemblyName);
		foreach (ModuleDefinition module in ad.Modules) {
			foreach (TypeDefinition type in module.GetAllTypes ()) {
				foreach (MethodDefinition method in type.Methods) {
					ProcessMethod (method, revision);
				}
			}
		}
	}

	static void SaveFile (string filename)
	{
		using (StreamWriter sw = new StreamWriter (filename)) {
			foreach (KeyValuePair<string, AuditData> kvp in Data) {
				// write only if Exists == true (i.e. stuff gets removed)
				if (!kvp.Value.Exists)
					continue;

				sw.WriteLine (kvp.Value.CurrentHash);
				sw.WriteLine (kvp.Key);
				foreach (KeyValuePair<int, string> comments in kvp.Value.Comments) {
					// don't prefix dates with 'r' after GIT migration and keep entries aligned
					sw.WriteLine ("\t{0}{1}\t{2}{3}", 
						comments.Key > 20100715 ? String.Empty : "r",
						comments.Key, 
						comments.Key > 20100715 ? String.Empty : "\t",
						comments.Value);
				}
				sw.WriteLine ();
			}
		}
		Data.Clear ();
	}

	static bool debug_log_enabled = false;
	static string debug_log_method = String.Empty;

	static int Main (string [] args)
	{
		if (args.Length < 3) {
			Console.WriteLine ("Usage: mono audit.exe platform_directory data_directory svn_revision");
			return 1;
		}

		string platorm_dir = args [0];
		if (!Directory.Exists (platorm_dir)) {
			Console.WriteLine ("Platform directory '{0}' not found.", platorm_dir);
			return 1;
		}

		string dest_dir = Path.Combine (Environment.CurrentDirectory, args [1]);
		if (!Directory.Exists (dest_dir)) {
			Console.WriteLine ("Data directory '{0}' not found.", dest_dir);
			return 1;
		}

		string rev = args [2];
		if (rev.StartsWith ("r"))
			rev = rev.Substring (1);
		int revision;
		if (!Int32.TryParse (rev, out revision)) {
			Console.WriteLine ("Invalid revision number '{0}'.", args [2]);
			return 1;
		}

		debug_log_method = Environment.GetEnvironmentVariable ("MOON_AUDIT_LOG");
		debug_log_enabled = !String.IsNullOrEmpty (debug_log_method);

		foreach (string assembly in PlatformCode.Assemblies) {
			string filename = Path.Combine (dest_dir, assembly + ".audit");

			// step 1 - read the audit file
			ReadFile (filename);

			// step 2 - process the assembly
			string assembly_name = Path.Combine (platorm_dir, assembly + ".dll");
			ProcessAssembly (assembly_name, revision);

			// step 3 - write the updated audit file
			SaveFile (filename);
		}
		return 0;
	}
}

