﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Markdown.Helpers;
using System.Windows.Controls.Markdown.Parse;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Documents;
using System.Linq;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace System.Windows.Controls.Markdown.Display
{
    internal class XamlRenderContext
    {
        public Brush Foreground { get; set; }

        public bool TrimLeadingWhitespace { get; set; }

        public bool WithinHyperlink { get; set; }

        public object DataContext { get; set; }

        public XamlRenderContext Clone()
        {
            return (XamlRenderContext)MemberwiseClone();
        }
    }

    internal class XamlRenderer
    {
        private static bool TextDecorationsSupported { get { return true; } }
        private static Dictionary<Type, CustomXamlInlineRenderer> CustomRenderers = new Dictionary<Type, CustomXamlInlineRenderer>();

        internal static void RegisterRenderer<TInline>(CustomXamlInlineRenderer renderer)
        {
            CustomRenderers[typeof(TInline)] = renderer;
        }

        internal static Brush Transparent = new SolidColorBrush(Colors.Transparent);

        /// <summary>
        /// The markdown document that will be rendered.
        /// </summary>
        private readonly MarkdownDocument _document;

        /// <summary>
        /// An interface that is used to register hyperlinks.
        /// </summary>
        private readonly ILinkRegister _linkRegister;

        /// <summary>
        /// An interface that is used to resolve images.
        /// </summary>
        private readonly IImageResolver _imageResolver;

        public XamlRenderer(MarkdownDocument document, ILinkRegister linkRegister, IImageResolver imageResolver, object dataContext)
        {
            _document = document;
            _linkRegister = linkRegister;
            _imageResolver = imageResolver;
            _dataContext = dataContext;
        }

        public List<RichTextBox> textBoxes = new List<RichTextBox>();

        public bool IsBasicMarkdown { get; set; }

        /// <summary>
        /// Gets or sets the stretch used for images.
        /// </summary>
        public Stretch ImageStretch { get; set; }

        /// <summary>
        /// Gets or sets a brush that provides the background of the control.
        /// </summary>
        public Brush Background { get; set; }

        /// <summary>
        /// Gets or sets a brush that describes the border fill of a control.
        /// </summary>
        public Brush BorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the border thickness of a control.
        /// </summary>
        public Thickness BorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the uniform spacing between characters, in units of 1/1000 of an em.
        /// </summary>
        public int CharacterSpacing { get; set; }

        /// <summary>
        /// Gets or sets the font used to display text in the control.
        /// </summary>
        public FontFamily FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the size of the text in this control.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the degree to which a font is condensed or expanded on the screen.
        /// </summary>
        public FontStretch FontStretch { get; set; }

        /// <summary>
        /// Gets or sets the style in which the text is rendered.
        /// </summary>
        public FontStyle FontStyle { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the specified font.
        /// </summary>
        public FontWeight FontWeight { get; set; }

        /// <summary>
        /// Gets or sets a brush that describes the foreground color.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether text selection is enabled.
        /// </summary>
        public bool IsTextSelectionEnabled { get; set; }

        /// <summary>
        /// Gets or sets the distance between the border and its child object.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the brush used to fill the background of a code block.
        /// </summary>
        public Brush CodeBackground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render the border fill of a code block.
        /// </summary>
        public Brush CodeBorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border around code blocks.
        /// </summary>
        public Thickness CodeBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render the text inside a code block.  If this is
        /// <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush CodeForeground { get; set; }

        /// <summary>
        /// Gets or sets the font used to display code.  If this is <c>null</c>, then
        /// <see cref="FontFamily"/> is used.
        /// </summary>
        public FontFamily CodeFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the space outside of code blocks.
        /// </summary>
        public Thickness CodeMargin { get; set; }

        /// <summary>
        /// Gets or sets the space between the code border and the text.
        /// </summary>
        public Thickness CodePadding { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 1 headers.
        /// </summary>
        public FontWeight Header1FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 1 headers.
        /// </summary>
        public double Header1FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 1 headers.
        /// </summary>
        public Thickness Header1Margin { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 1 headers.
        /// </summary>
        public Brush Header1Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 2 headers.
        /// </summary>
        public FontWeight Header2FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 2 headers.
        /// </summary>
        public double Header2FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 2 headers.
        /// </summary>
        public Thickness Header2Margin { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 2 headers.
        /// </summary>
        public Brush Header2Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 3 headers.
        /// </summary>
        public FontWeight Header3FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 3 headers.
        /// </summary>
        public double Header3FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 3 headers.
        /// </summary>
        public Thickness Header3Margin { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 3 headers.
        /// </summary>
        public Brush Header3Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 4 headers.
        /// </summary>
        public FontWeight Header4FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 4 headers.
        /// </summary>
        public double Header4FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 4 headers.
        /// </summary>
        public Thickness Header4Margin { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 4 headers.
        /// </summary>
        public Brush Header4Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 5 headers.
        /// </summary>
        public FontWeight Header5FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 5 headers.
        /// </summary>
        public double Header5FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 5 headers.
        /// </summary>
        public Thickness Header5Margin { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 5 headers.
        /// </summary>
        public Brush Header5Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font weight to use for level 6 headers.
        /// </summary>
        public FontWeight Header6FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font size for level 6 headers.
        /// </summary>
        public double Header6FontSize { get; set; }

        /// <summary>
        /// Gets or sets the margin for level 6 headers.
        /// </summary>
        public Thickness Header6Margin { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for level 6 headers.
        /// </summary>
        public Brush Header6Foreground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render a horizontal rule.  If this is <c>null</c>, then
        /// <see cref="Foreground"/> is used.
        /// </summary>
        public Brush HorizontalRuleBrush { get; set; }

        /// <summary>
        /// Gets or sets the margin used for horizontal rules.
        /// </summary>
        public Thickness HorizontalRuleMargin { get; set; }

        /// <summary>
        /// Gets or sets the vertical thickness of the horizontal rule.
        /// </summary>
        public double HorizontalRuleThickness { get; set; }

        /// <summary>
        /// Gets or sets the margin used by lists.
        /// </summary>
        public Thickness ListMargin { get; set; }

        /// <summary>
        /// Gets or sets the width of the space used by list item bullets/numbers.
        /// </summary>
        public double ListGutterWidth { get; set; }

        /// <summary>
        /// Gets or sets the space between the list item bullets/numbers and the list item content.
        /// </summary>
        public double ListBulletSpacing { get; set; }

        /// <summary>
        /// Gets or sets the margin used for paragraphs.
        /// </summary>
        public Thickness ParagraphMargin { get; set; }

        /// <summary>
        /// Gets or sets the brush used to fill the background of a quote block.
        /// </summary>
        public Brush QuoteBackground { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render a quote border.  If this is <c>null</c>, then
        /// <see cref="Foreground"/> is used.
        /// </summary>
        public Brush QuoteBorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the thickness of quote borders.
        /// </summary>
        public Thickness QuoteBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render the text inside a quote block.  If this is
        /// <c>null</c>, then <see cref="Foreground"/> is used.
        /// </summary>
        public Brush QuoteForeground { get; set; }

        /// <summary>
        /// Gets or sets the space outside of quote borders.
        /// </summary>
        public Thickness QuoteMargin { get; set; }

        /// <summary>
        /// Gets or sets the space between the quote border and the text.
        /// </summary>
        public Thickness QuotePadding { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render table borders.  If this is <c>null</c>, then
        /// <see cref="Foreground"/> is used.
        /// </summary>
        public Brush TableBorderBrush { get; set; }

        /// <summary>
        /// Gets or sets the thickness of any table borders.
        /// </summary>
        public double TableBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the padding inside each cell.
        /// </summary>
        public Thickness TableCellPadding { get; set; }

        /// <summary>
        /// Gets or sets the margin used by tables.
        /// </summary>
        public Thickness TableMargin { get; set; }

        /// <summary>
        /// Gets or sets the word wrapping behavior.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        public double CodeFontSize { get; set; }

        /// <summary>
        /// Gets or sets the brush used to render links.  If this is <c>null</c>, then
        /// <see cref="Foreground"/> is used.
        /// </summary>
        public Brush LinkForeground { get; set; }

        private StackPanel _root;
        private object _dataContext;

        /// <summary>
        /// Called externally to render markdown to a text block.
        /// </summary>
        /// <returns> A XAML UI element. </returns>
        public UIElement Render()
        {
            _root = new StackPanel();
            RenderBlocks(_document.Blocks, _root.Children, new XamlRenderContext { Foreground = Foreground, DataContext = _dataContext });

            // Set background and border properties.
            _root.Background = Background;

            return _root;
        }

        /// <summary>
        /// Renders a list of block elements.
        /// </summary>
        private void RenderBlocks(IEnumerable<MarkdownBlock> blockElements, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            foreach (MarkdownBlock element in blockElements)
            {
                RenderBlock(element, blockUIElementCollection, context);
            }

            // Remove the top margin from the first block element, the bottom margin from the last block element,
            // and collapse adjacent margins.
            FrameworkElement previousFrameworkElement = null;
            for (int i = 0; i < blockUIElementCollection.Count; i++)
            {
                FrameworkElement frameworkElement = blockUIElementCollection[i] as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (i == 0)
                    {
                        // Remove the top margin.
                        frameworkElement.Margin = new Thickness(
                            frameworkElement.Margin.Left,
                            0,
                            frameworkElement.Margin.Right,
                            frameworkElement.Margin.Bottom);
                    }
                    else if (previousFrameworkElement != null)
                    {
                        // Remove the bottom margin.
                        frameworkElement.Margin = new Thickness(
                            frameworkElement.Margin.Left,
                            Math.Max(frameworkElement.Margin.Top, previousFrameworkElement.Margin.Bottom),
                            frameworkElement.Margin.Right,
                            frameworkElement.Margin.Bottom);
                        previousFrameworkElement.Margin = new Thickness(
                            previousFrameworkElement.Margin.Left,
                            previousFrameworkElement.Margin.Top,
                            previousFrameworkElement.Margin.Right,
                            0);
                    }
                }

                previousFrameworkElement = frameworkElement;
            }
        }

        /// <summary>
        /// Called to render a block element.
        /// </summary>
        private void RenderBlock(MarkdownBlock element, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            switch (element.Type)
            {
                case MarkdownBlockType.Paragraph:
                    RenderParagraph((ParagraphBlock)element, blockUIElementCollection, context);
                    break;
                case MarkdownBlockType.Quote:
                    RenderQuote((QuoteBlock)element, blockUIElementCollection, context);
                    break;
                case MarkdownBlockType.Code:
                    RenderCode((CodeBlock)element, blockUIElementCollection, context);
                    break;
                case MarkdownBlockType.Header:
                    if (!IsBasicMarkdown)
                        RenderHeader((HeaderBlock)element, blockUIElementCollection, context);
                    break;
                case MarkdownBlockType.List:
                    if (!IsBasicMarkdown)
                        RenderListElement((ListBlock)element, blockUIElementCollection, context);
                    break;
                case MarkdownBlockType.HorizontalRule:
                    if (!IsBasicMarkdown)
                        RenderHorizontalRule(blockUIElementCollection, context);
                    break;
                case MarkdownBlockType.Table:
                    if (!IsBasicMarkdown)
                        RenderTable((TableBlock)element, blockUIElementCollection, context);
                    break;
            }
        }

        /// <summary>
        /// Renders a paragraph element.
        /// </summary>
        private void RenderParagraph(ParagraphBlock element, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            Paragraph paragraph = new Paragraph
            {
                //Margin = ParagraphMargin
            };
            context.TrimLeadingWhitespace = true;
            RenderInlineChildren(paragraph.Inlines, element.Inlines, paragraph, context);

            RichTextBox textBlock = CreateOrReuseRichTextBlock(blockUIElementCollection, context);
            textBlock.Blocks.Add(paragraph);
        }

        /// <summary>
        /// Renders a header element.
        /// </summary>
        private void RenderHeader(HeaderBlock element, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            RichTextBox textBlock = CreateOrReuseRichTextBlock(blockUIElementCollection, context);

            Paragraph paragraph = new Paragraph();
            InlineCollection childInlines = paragraph.Inlines;
            switch (element.HeaderLevel)
            {
                case 1:
                    //paragraph.Margin = Header1Margin;
                    paragraph.FontSize = Header1FontSize;
                    paragraph.FontWeight = Header1FontWeight;
                    paragraph.Foreground = Header1Foreground;
                    break;
                case 2:
                    //paragraph.Margin = Header2Margin;
                    paragraph.FontSize = Header2FontSize;
                    paragraph.FontWeight = Header2FontWeight;
                    paragraph.Foreground = Header2Foreground;
                    break;
                case 3:
                    //paragraph.Margin = Header3Margin;
                    paragraph.FontSize = Header3FontSize;
                    paragraph.FontWeight = Header3FontWeight;
                    paragraph.Foreground = Header3Foreground;
                    break;
                case 4:
                    //paragraph.Margin = Header4Margin;
                    paragraph.FontSize = Header4FontSize;
                    paragraph.FontWeight = Header4FontWeight;
                    paragraph.Foreground = Header4Foreground;
                    break;
                case 5:
                    //paragraph.Margin = Header5Margin;
                    paragraph.FontSize = Header5FontSize;
                    paragraph.FontWeight = Header5FontWeight;
                    paragraph.Foreground = Header5Foreground;
                    break;
                case 6:
                    //paragraph.Margin = Header6Margin;
                    paragraph.FontSize = Header6FontSize;
                    paragraph.FontWeight = Header6FontWeight;
                    paragraph.Foreground = Header6Foreground;

                    Underline underline = new Underline();
                    childInlines = underline.Inlines;
                    paragraph.Inlines.Add(underline);
                    break;
            }

            // Render the children into the para inline.
            context.TrimLeadingWhitespace = true;
            RenderInlineChildren(childInlines, element.Inlines, paragraph, context);

            // Add it to the blocks
            textBlock.Blocks.Add(paragraph);
        }

        /// <summary>
        /// Renders a list element.
        /// </summary>
        private void RenderListElement(ListBlock element, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            // Create a grid with two columns.
            Grid grid = new Grid
            {
                Margin = ListMargin
            };

            // The first column for the bullet (or number) and the second for the text.
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ListGutterWidth) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            for (int rowIndex = 0; rowIndex < element.Items.Count; rowIndex++)
            {
                ListBlock.ListItemBlock listItem = element.Items[rowIndex];

                // Add a row definition.
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // Add the bullet or number.
                TextBlock bullet = CreateTextBlock(context);
                bullet.Margin = ParagraphMargin;
                switch (element.Style)
                {
                    case ListStyle.Bulleted:
                        bullet.Text = "•";
                        break;
                    case ListStyle.Numbered:
                        bullet.Text = string.Format("{0}.", rowIndex + 1);
                        break;
                }

                bullet.HorizontalAlignment = HorizontalAlignment.Right;
                bullet.Margin = new Thickness(0, 0, ListBulletSpacing, 0);
                Grid.SetRow(bullet, rowIndex);
                grid.Children.Add(bullet);

                // Add the list item content.
                StackPanel content = new StackPanel();
                RenderBlocks(listItem.Blocks, content.Children, context);
                Grid.SetColumn(content, 1);
                Grid.SetRow(content, rowIndex);
                grid.Children.Add(content);
            }

            blockUIElementCollection.Add(grid);
        }

        /// <summary>
        /// Renders a horizontal rule element.
        /// </summary>
        private void RenderHorizontalRule(UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            Rectangle rectangle = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = HorizontalRuleThickness,
                Fill = HorizontalRuleBrush ?? context.Foreground,
                Margin = HorizontalRuleMargin
            };

            blockUIElementCollection.Add(rectangle);
        }

        /// <summary>
        /// Renders a quote element.
        /// </summary>
        private void RenderQuote(QuoteBlock element, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            if (QuoteForeground != null)
            {
                context = context.Clone();
                context.Foreground = QuoteForeground;
            }

            StackPanel stackPanel = new StackPanel();
            RenderBlocks(element.Blocks, stackPanel.Children, context);

            Border border = new Border
            {
                Margin = QuoteMargin,
                Background = QuoteBackground,
                BorderBrush = QuoteBorderBrush ?? context.Foreground,
                BorderThickness = QuoteBorderThickness,
                Padding = QuotePadding,
                Child = stackPanel
            };

            blockUIElementCollection.Add(border);
        }

        /// <summary>
        /// Renders a code element.
        /// </summary>
        private void RenderCode(CodeBlock element, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            TextBlock textBlock = CreateTextBlock(context);
            textBlock.FontFamily = CodeFontFamily ?? FontFamily;
            textBlock.Foreground = CodeForeground ?? context.Foreground;
            textBlock.FontSize = CodeFontSize;
            //textBlock.LineHeight = CodeFontSize * 1.4;
            textBlock.Text = element.Text;

            Border border = new Border
            {
                Background = CodeBackground,
                BorderBrush = CodeBorderBrush,
                BorderThickness = CodeBorderThickness,
                Padding = CodePadding,
                Margin = CodeMargin,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Child = textBlock
            };

            // Add it to the blocks
            blockUIElementCollection.Add(border);
        }

        /// <summary>
        /// Renders a table element.
        /// </summary>
        private void RenderTable(TableBlock element, UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            MarkdownTable table = new MarkdownTable(element.ColumnDefinitions.Count, element.Rows.Count, TableBorderThickness, TableBorderBrush)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = TableMargin
            };

            // Add each row.
            for (int rowIndex = 0; rowIndex < element.Rows.Count; rowIndex++)
            {
                TableBlock.TableRow row = element.Rows[rowIndex];

                // Add each cell.
                for (int cellIndex = 0; cellIndex < Math.Min(element.ColumnDefinitions.Count, row.Cells.Count); cellIndex++)
                {
                    TableBlock.TableCell cell = row.Cells[cellIndex];

                    // Cell content.
                    RichTextBox cellContent = CreateOrReuseRichTextBlock(null, context);
                    cellContent.Margin = TableCellPadding;
                    Grid.SetRow(cellContent, rowIndex);
                    Grid.SetColumn(cellContent, cellIndex);

                    if (rowIndex == 0)
                    {
                        cellContent.FontWeight = FontWeights.Bold;
                    }

                    Paragraph paragraph = new Paragraph();
                    context.TrimLeadingWhitespace = true;
                    RenderInlineChildren(paragraph.Inlines, cell.Inlines, paragraph, context);
                    cellContent.Blocks.Add(paragraph);
                    table.Children.Add(cellContent);
                }
            }

            blockUIElementCollection.Add(table);
        }

        /// <summary>
        /// Renders all of the children for the given element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="inlineElements"> The parsed inline elements to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        internal void RenderInlineChildren(InlineCollection inlineCollection, IList<MarkdownInline> inlineElements, TextElement parent, XamlRenderContext context)
        {
            foreach (MarkdownInline element in inlineElements)
            {
                RenderInline(inlineCollection, element, parent, context);
            }
        }

        /// <summary>
        /// Called to render an inline element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderInline(InlineCollection inlineCollection, MarkdownInline element, TextElement parent, XamlRenderContext context)
        {
            switch (element.Type)
            {
                case MarkdownInlineType.TextRun:
                    RenderTextRun(inlineCollection, (TextRunInline)element, context);
                    break;
                case MarkdownInlineType.Italic:
                    RenderItalicRun(inlineCollection, (ItalicTextInline)element, context);
                    break;
                case MarkdownInlineType.Bold:
                    RenderBoldRun(inlineCollection, (BoldTextInline)element, context);
                    break;
                case MarkdownInlineType.Underline:
                    RenderUnderlineRun(inlineCollection, (UnderlineTextInline)element, context);
                    break;
                case MarkdownInlineType.MarkdownLink:
                    RenderMarkdownLink(inlineCollection, (MarkdownLinkInline)element, parent, context);
                    break;
                case MarkdownInlineType.RawHyperlink:
                    RenderHyperlink(inlineCollection, (HyperlinkInline)element, context);
                    break;
                case MarkdownInlineType.Strikethrough:
                    RenderStrikethroughRun(inlineCollection, (StrikethroughTextInline)element, context);
                    break;
                case MarkdownInlineType.Superscript:
                    RenderSuperscriptRun(inlineCollection, (SuperscriptTextInline)element, parent, context);
                    break;
                case MarkdownInlineType.Code:
                    RenderCodeRun(inlineCollection, (CodeInline)element, context);
                    break;
                case MarkdownInlineType.Image:
                    if (!IsBasicMarkdown)
                        RenderImage(inlineCollection, (ImageInline)element, context);
                    break;
                case MarkdownInlineType.Custom:
                    CustomRenderers[element.GetType()].Render(new CustomXamlInlineRenderContext(this, context, inlineCollection, parent), element);
                    break;
            }
        }

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        /// <returns><see cref="Run"/></returns>
        private Run RenderTextRun(InlineCollection inlineCollection, TextRunInline element, XamlRenderContext context)
        {
            // Create the text run
            Run textRun = new Run
            {
                Text = CollapseWhitespace(context, element.Text)
            };

            // Add it
            inlineCollection.Add(textRun);

            return textRun;
        }

        /// <summary>
        /// Renders a bold run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderBoldRun(InlineCollection inlineCollection, BoldTextInline element, XamlRenderContext context)
        {
            // Create the text run
            Span boldSpan = new Span
            {
                FontWeight = FontWeights.Bold
            };

            // Render the children into the bold inline.
            RenderInlineChildren(boldSpan.Inlines, element.Inlines, boldSpan, context);

            // Add it to the current inlines
            inlineCollection.Add(boldSpan);
        }

        /// <summary>
        /// Renders an underlined run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderUnderlineRun(InlineCollection inlineCollection, UnderlineTextInline element, XamlRenderContext context)
        {
            // Create the text run
            Span boldSpan = new Span
            {
                TextDecorations = TextDecorations.Underline
            };

            // Render the children into the underline inline.
            RenderInlineChildren(boldSpan.Inlines, element.Inlines, boldSpan, context);

            // Add it to the current inlines
            inlineCollection.Add(boldSpan);
        }

        /// <summary>
        /// Renders a link element
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderMarkdownLink(InlineCollection inlineCollection, MarkdownLinkInline element, TextElement parent, XamlRenderContext context)
        {
            // Avoid crash when link text is empty.
            if (element.Inlines.Count == 0)
            {
                return;
            }

            // Attempt to resolve references.
            element.ResolveReference(_document);
            if (element.Url == null)
            {
                // The element couldn't be resolved, just render it as text.
                RenderInlineChildren(inlineCollection, element.Inlines, parent, context);
                return;
            }

            // HACK: Superscript is not allowed within a hyperlink.  But if we switch it around, so
            // that the superscript is outside the hyperlink, then it will render correctly.
            // This assumes that the entire hyperlink is to be rendered as superscript.
            if (AllTextIsSuperscript(element) == false)
            {
                // Regular ol' hyperlink.
                Hyperlink link = new Hyperlink();

                // Register the link
                _linkRegister.RegisterNewHyperLink(link, element.Url);

                // Remove superscripts.
                RemoveSuperscriptRuns(element, insertCaret: true);

                // Render the children into the link inline.
                XamlRenderContext childContext = context.Clone();
                childContext.WithinHyperlink = true;

                if (LinkForeground != null)
                {
                    link.Foreground = LinkForeground;
                }

                RenderInlineChildren(link.Inlines, element.Inlines, link, childContext);
                context.TrimLeadingWhitespace = childContext.TrimLeadingWhitespace;

                // Add it to the current inlines
                inlineCollection.Add(link);
            }
            else
            {
                // THE HACK IS ON!

                // Create a fake superscript element.
                SuperscriptTextInline fakeSuperscript = new SuperscriptTextInline
                {
                    Inlines = new List<MarkdownInline>
                    {
                        element
                    }
                };

                // Remove superscripts.
                RemoveSuperscriptRuns(element, insertCaret: false);

                // Now render it.
                RenderSuperscriptRun(inlineCollection, fakeSuperscript, parent, context);
            }
        }

        /// <summary>
        /// Renders an image element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        private async void RenderImage(InlineCollection inlineCollection, ImageInline element, XamlRenderContext context)
        {
            Run placeholder = RenderTextRun(inlineCollection, new TextRunInline { Text = element.Text, Type = MarkdownInlineType.TextRun }, context);

            ImageSource resolvedImage = await _imageResolver.ResolveImageAsync(element.Url, element.Tooltip);

            // if image can not be resolved we have to return
            if (resolvedImage == null)
            {
                return;
            }

            Image image = new Image();
            InlineUIContainer imageContainer = new InlineUIContainer() { Child = image };

            image.Source = resolvedImage;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Stretch = ImageStretch;

            //image.ToolTip = new ToolTip() { Content = element.Tooltip };

            // Try to add it to the current inlines
            // Could fail because some containers like Hyperlink cannot have inlined images
            try
            {
                int placeholderIndex = inlineCollection.ToList().IndexOf(placeholder);
                inlineCollection.Insert(placeholderIndex + 1, imageContainer);
                inlineCollection.Remove(placeholder);
            }
            catch
            {
                // Ignore error
            }
        }

        /// <summary>
        /// Renders a raw link element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderHyperlink(InlineCollection inlineCollection, HyperlinkInline element, XamlRenderContext context)
        {
            Hyperlink link = new Hyperlink();

            // Register the link
            _linkRegister.RegisterNewHyperLink(link, element.Url);

            // Make a text block for the link
            Run linkText = new Run
            {
                Text = CollapseWhitespace(context, element.Text)
            };

            link.Inlines.Add(linkText);

            // Add it to the current inlines
            inlineCollection.Add(link);
        }

        /// <summary>
        /// Renders a text run element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderItalicRun(InlineCollection inlineCollection, ItalicTextInline element, XamlRenderContext context)
        {
            // Create the text run
            Span italicSpan = new Span
            {
                FontStyle = FontStyles.Italic
            };

            // Render the children into the italic inline.
            RenderInlineChildren(italicSpan.Inlines, element.Inlines, italicSpan, context);

            // Add it to the current inlines
            inlineCollection.Add(italicSpan);
        }

        /// <summary>
        /// Renders a strikethrough element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderStrikethroughRun(InlineCollection inlineCollection, StrikethroughTextInline element, XamlRenderContext context)
        {
            Span span = new Span();
            span.FontFamily = new FontFamily("Consolas");

            // Render the children into the inline.
            RenderInlineChildren(span.Inlines, element.Inlines, span, context);

            AlterChildRuns(span, (parentSpan, run) =>
            {
                string text = run.Text;
                StringBuilder builder = new StringBuilder(text.Length * 2);
                foreach (char c in text)
                {
                    builder.Append((char)0x0336);
                    builder.Append(c);
                }
                run.Text = builder.ToString();
            });


            // Add it to the current inlines
            inlineCollection.Add(span);
        }

        /// <summary>
        /// Renders a superscript element.
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="parent"> The container element. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderSuperscriptRun(InlineCollection inlineCollection, SuperscriptTextInline element, TextElement parent, XamlRenderContext context)
        {
            // Le <sigh>, InlineUIContainers are not allowed within hyperlinks.
            if (context.WithinHyperlink)
            {
                RenderInlineChildren(inlineCollection, element.Inlines, parent, context);
                return;
            }

            Paragraph paragraph = new Paragraph
            {
                FontSize = parent.FontSize * 0.8,
                FontFamily = parent.FontFamily,
                FontStyle = parent.FontStyle,
                FontWeight = parent.FontWeight
            };
            RenderInlineChildren(paragraph.Inlines, element.Inlines, paragraph, context);

            RichTextBox richTextBlock = CreateOrReuseRichTextBlock(null, context);
            richTextBlock.Blocks.Add(paragraph);

            InlineUIContainer inlineUIContainer = new InlineUIContainer
            {
                Child = richTextBlock,
                // BaselineAlignment = BaselineAlignment.Top
            };

            // Add it to the current inlines
            inlineCollection.Add(inlineUIContainer);
        }

        /// <summary>
        /// Renders a code element
        /// </summary>
        /// <param name="inlineCollection"> The list to add to. </param>
        /// <param name="element"> The parsed inline element to render. </param>
        /// <param name="context"> Persistent state. </param>
        private void RenderCodeRun(InlineCollection inlineCollection, CodeInline element, XamlRenderContext context)
        {
            var border = new Border()
            {
                Padding = new Thickness(2),
                Background = CodeBackground,
                RenderTransform = new TranslateTransform() { Y = 4 },
                Child = new TextBlock()
                {
                    FontFamily = CodeFontFamily ?? FontFamily,
                    FontSize = CodeFontSize,
                    Text = CollapseWhitespace(context, element.Text),
                    TextWrapping = TextWrapping.Wrap
                }
            };

            InlineUIContainer inline = new InlineUIContainer()
            {
                Child = border,
                // BaselineAlignment = BaselineAlignment.Center
            };

            // Add it to the current inlines
            inlineCollection.Add(inline);
        }

        /// <summary>
        /// Performs an action against any runs that occur within the given span.
        /// </summary>
        private void AlterChildRuns(Span parentSpan, Action<Span, Run> action)
        {
            foreach (Inline inlineElement in parentSpan.Inlines)
            {
                Span span = inlineElement as Span;
                if (span != null)
                {
                    AlterChildRuns(span, action);
                }
                else if (inlineElement is Run)
                {
                    action(parentSpan, (Run)inlineElement);
                }
            }
        }

        /// <summary>
        /// Removes leading whitespace, but only if this is the first run in the block.
        /// </summary>
        /// <returns>The corrected string</returns>
        private string CollapseWhitespace(XamlRenderContext context, string text)
        {
            bool dontOutputWhitespace = context.TrimLeadingWhitespace;
            StringBuilder result = null;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == ' ' || c == '\t')
                {
                    if (dontOutputWhitespace)
                    {
                        if (result == null)
                        {
                            result = new StringBuilder(text.Substring(0, i), text.Length);
                        }
                    }
                    else
                    {
                        if (result != null)
                            result.Append(c);

                        dontOutputWhitespace = true;
                    }
                }
                else
                {
                    if (result != null)
                        result.Append(c);

                    dontOutputWhitespace = false;
                }
            }

            context.TrimLeadingWhitespace = false;
            return result == null ? text : result.ToString();
        }

        /// <summary>
        /// Creates a new RichTextBlock, if the last element of the provided collection isn't already a RichTextBlock.
        /// </summary>
        /// <returns>The rich text block</returns>
        private RichTextBox CreateOrReuseRichTextBlock(UIElementCollection blockUIElementCollection, XamlRenderContext context)
        {
            // Reuse the last RichTextBlock, if possible.
            if (blockUIElementCollection != null && blockUIElementCollection.Count > 0 && blockUIElementCollection[blockUIElementCollection.Count - 1] is RichTextBox)
            {
                return (RichTextBox)blockUIElementCollection[blockUIElementCollection.Count - 1];
            }

            RichTextBox result = new RichTextBox
            {
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Foreground = context.Foreground,
                IsReadOnly = true,
                //IsDocumentEnabled = true,
                Background = Transparent,
                BorderBrush = Transparent,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                // IsUndoEnabled = false,
                AcceptsReturn = false,
                // AcceptsTab = false
            };

            result.Blocks.Clear();
            //result.IsOptimalParagraphEnabled = false;

            if (blockUIElementCollection != null)
                blockUIElementCollection.Add(result);

            textBoxes.Add(result);

            return result;
        }

        /// <summary>
        /// Creates a new TextBlock, with default settings.
        /// </summary>
        /// <returns>The created TextBlock</returns>
        private TextBlock CreateTextBlock(XamlRenderContext context)
        {
            TextBlock result = new TextBlock
            {
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Foreground = context.Foreground,
                TextWrapping = TextWrapping,
                LineStackingStrategy = LineStackingStrategy.BlockLineHeight
            };
            return result;
        }

        /// <summary>
        /// Checks if all text elements inside the given container are superscript.
        /// </summary>
        /// <returns> <c>true</c> if all text is superscript (level 1); <c>false</c> otherwise. </returns>
        private bool AllTextIsSuperscript(IInlineContainer container, int superscriptLevel = 0)
        {
            foreach (MarkdownInline inline in container.Inlines)
            {
                SuperscriptTextInline textInline = inline as SuperscriptTextInline;
                if (textInline != null)
                {
                    // Remove any nested superscripts.
                    if (AllTextIsSuperscript(textInline, superscriptLevel + 1) == false)
                    {
                        return false;
                    }
                }
                else if (inline is IInlineContainer)
                {
                    // Remove any superscripts.
                    if (AllTextIsSuperscript((IInlineContainer)inline, superscriptLevel) == false)
                    {
                        return false;
                    }
                }
                else if (inline is IInlineLeaf && !ParseHelpers.IsBlankOrWhiteSpace(((IInlineLeaf)inline).Text))
                {
                    if (superscriptLevel != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Removes all superscript elements from the given container.
        /// </summary>
        private void RemoveSuperscriptRuns(IInlineContainer container, bool insertCaret)
        {
            for (int i = 0; i < container.Inlines.Count; i++)
            {
                MarkdownInline inline = container.Inlines[i];
                SuperscriptTextInline textInline = inline as SuperscriptTextInline;
                if (textInline != null)
                {
                    // Remove any nested superscripts.
                    RemoveSuperscriptRuns(textInline, insertCaret);

                    // Remove the superscript element, insert all the children.
                    container.Inlines.RemoveAt(i);
                    if (insertCaret)
                    {
                        container.Inlines.Insert(i++, new TextRunInline { Text = "^" });
                    }

                    foreach (MarkdownInline superscriptInline in textInline.Inlines)
                    {
                        container.Inlines.Insert(i++, superscriptInline);
                    }

                    i--;
                }
                else if (inline is IInlineContainer)
                {
                    // Remove any superscripts.
                    RemoveSuperscriptRuns((IInlineContainer)inline, insertCaret);
                }
            }
        }
    }
}
