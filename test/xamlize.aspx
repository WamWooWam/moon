<%@ page language="C#" %>
<html>
	<head>
		<script type="text/javascript" src="Silverlight.js"></script>
		<script lang="text/javascript">
function createSilverlight()
{
	Silverlight.createObjectEx({
		source: "<%= Request ["xaml"] %>",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "100%",
			height: "100%",
			version: "1.1",
			enableHtmlAccess: "true"
		},
		events: {}
	});
	   
	// Give the keyboard focus to the Silverlight control by default
    document.body.onload = function() {
      var silverlightControl = document.getElementById('SilverlightControl');
      if (silverlightControl)
      silverlightControl.focus();
    }

}
		</script>
		<style type="text/css">
.silverlightHost { }
		</style>
	</head>
    
	<body bgcolor="gray">

		<div id="SilverlightControlHost" class="silverlightHost" >
			<script type="text/javascript">
				createSilverlight();
			</script>
		</div>
	</body>
</html>

