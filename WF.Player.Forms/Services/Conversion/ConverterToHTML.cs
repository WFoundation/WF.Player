// <copyright file="ConverterToHTML.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// </copyright>

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace WF.Player
{
	using System;
	using System.Text;
	using WF.Player.Core;
	using Xamarin.Forms;

	/// <summary>
	/// Convert to html.
	/// </summary>
	public class ConverterToHtml
	{
		private static MarkdownSharp.Markdown markdownConverter = new MarkdownSharp.Markdown();

		// Found at https://gist.github.com/ChaseFlorell/7211ffd025f4befcfa9e
		private const string DefaultStyle = @"
body {
font-family: Helvetica, arial, sans-serif;
font-size: -Font1-px;
text-align: -TextAlign-;
line-height: 1.0;
padding-top: 10px;
padding-bottom: 10px;
background-color: -BackgroundColor-;
padding: 30px;
margin: 10px;
color: -TextColor-;
}
body > *:first-child {
margin-top: 0 !important;
}
body > *:last-child {
margin-bottom: 0 !important;
}
a {
color: #4183C4;
text-decoration: none;
}
a.absent {
color: #cc0000;
}
a.anchor {
display: block;
padding-left: 30px;
margin-left: -30px;
cursor: pointer;
position: absolute;
top: 0;
left: 0;
bottom: 0;
}
h1, h2, h3, h4, h5, h6 {
margin: 20px 0 10px;
padding: 0;
font-weight: bold;
-webkit-font-smoothing: antialiased;
cursor: text;
position: relative;
}
h2:first-child, h1:first-child, h1:first-child + h2, h3:first-child, h4:first-child, h5:first-child, h6:first-child {
margin-top: 0;
padding-top: 0;
}
h1:hover a.anchor, h2:hover a.anchor, h3:hover a.anchor, h4:hover a.anchor, h5:hover a.anchor, h6:hover a.anchor {
text-decoration: none;
}
h1 tt, h1 code {
font-size: inherit;
}
h2 tt, h2 code {
font-size: inherit;
}
h3 tt, h3 code {
font-size: inherit;
}
h4 tt, h4 code {
font-size: inherit;
}
h5 tt, h5 code {
font-size: inherit;
}
h6 tt, h6 code {
font-size: inherit;
}
h1 {
font-size: -Font5-px;
}
h2 {
font-size: -Font4-px;
border-bottom: 1px solid #cccccc;
}
h3 {
font-size: -Font3-px;
}
h4 {
font-size: -Font2-px;
}
h5 {
font-size: -Font1-px;
}
h6 {
color: #777777;
font-size: -Font1-px;
}
p, blockquote, ul, ol, dl, li, table, pre {
margin: 15px 0;
}
hr {
border: 0 none;
color: #222222;
height: 4px;
padding: 0;
}
body > h2:first-child {
margin-top: 0;
padding-top: 0;
}
body > h1:first-child {
margin-top: 0;
padding-top: 0;
}
body > h1:first-child + h2 {
margin-top: 0;
padding-top: 0;
}
body > h3:first-child, body > h4:first-child, body > h5:first-child, body > h6:first-child {
margin-top: 0;
padding-top: 0;
}
a:first-child h1, a:first-child h2, a:first-child h3, a:first-child h4, a:first-child h5, a:first-child h6 {
margin-top: 0;
padding-top: 0;
}
h1 p, h2 p, h3 p, h4 p, h5 p, h6 p {
margin-top: 0;
}
li p.first {
display: inline-block;
}
ul, ol {
padding-left: 30px;
}
ul :first-child, ol :first-child {
margin-top: 0;
}
ul :last-child, ol :last-child {
margin-bottom: 0;
}
dl {
padding: 0;
}
dl dt {
font-size: -Font1-px;
font-weight: bold;
font-style: italic;
padding: 0;
margin: 15px 0 5px;
}
dl dt:first-child {
padding: 0;
}
dl dt > :first-child {
margin-top: 0;
}
dl dt > :last-child {
margin-bottom: 0;
}
dl dd {
margin: 0 0 15px;
padding: 0 15px;
}
dl dd > :first-child {
margin-top: 0;
}
dl dd > :last-child {
margin-bottom: 0;
}
blockquote {
border-left: 4px solid #dddddd;
padding: 15px;
margin: 15px;
color: #777777;
}
blockquote > :first-child {
margin-top: 0;
}
blockquote > :last-child {
margin-bottom: 0;
}
table {
padding: 0;
}
table tr {
border-top: 1px solid #cccccc;
background-color: -BackgroundColor-;
margin: 0;
padding: 0;
}
table tr:nth-child(2n) {
background-color: #f8f8f8;
}
table tr th {
font-weight: bold;
border: 1px solid #cccccc;
text-align: left;
margin: 0;
padding: 6px 13px;
}
table tr td {
border: 1px solid #cccccc;
text-align: left;
margin: 0;
padding: 6px 13px;
}
table tr th :first-child, table tr td :first-child {
margin-top: 0;
}
table tr th :last-child, table tr td :last-child {
margin-bottom: 0;
}
img {
max-width: 100%;
}
span.frame {
display: block;
overflow: hidden;
}
span.frame > span {
border: 1px solid #dddddd;
display: block;
float: left;
overflow: hidden;
margin: 13px 0 0;
padding: 7px;
width: auto;
}
span.frame span img {
display: block;
float: left;
}
span.frame span span {
clear: both;
color: #333333;
display: block;
padding: 5px 0 0;
}
span.align-center {
display: block;
overflow: hidden;
clear: both;
}
span.align-center > span {
display: block;
overflow: hidden;
margin: 13px auto 0;
text-align: center;
}
span.align-center span img {
margin: 0 auto;
text-align: center;
}
span.align-right {
display: block;
overflow: hidden;
clear: both;
}
span.align-right > span {
display: block;
overflow: hidden;
margin: 13px 0 0;
text-align: right;
}
span.align-right span img {
margin: 0;
text-align: right;
}
span.float-left {
display: block;
margin-right: 13px;
overflow: hidden;
float: left;
}
span.float-left span {
margin: 13px 0 0;
}
span.float-right {
display: block;
margin-left: 13px;
overflow: hidden;
float: right;
}
span.float-right > span {
display: block;
overflow: hidden;
margin: 13px auto 0;
text-align: right;
}
code, tt {
margin: 0 2px;
padding: 0 5px;
white-space: nowrap;
border: 1px solid #eaeaea;
background-color: #f8f8f8;
border-radius: 3px;
}
pre code {
margin: 0;
padding: 0;
white-space: pre;
border: none;
background: transparent;
}
.highlight pre {
background-color: #f8f8f8;
border: 1px solid #cccccc;
font-size: -Font0-px;
line-height: 19px;
overflow: auto;
padding: 6px 10px;
border-radius: 3px;
}
pre {
background-color: #f8f8f8;
border: 1px solid #cccccc;
font-size: -Font0-px;
line-height: 19px;
overflow: auto;
padding: 6px 10px;
border-radius: 3px;
}
pre code, pre tt {
background-color: transparent;
border: none;
}";

		/// <summary>
		/// Create HTML from plain text and media.
		/// </summary>
		/// <returns>Text converted to HTML.</returns>
		/// <param name="text">Original text.</param>
		/// <param name="media">Media object.</param>
		public static string FromText(string text, Media media = null)
		{
			text.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br>");

			return ConvertStringToHTML(text, media);
		}

		/// <summary>
		/// Create HTML froms markdown text and media.
		/// </summary>
		/// <returns>Markdown converted to HTML.</returns>
		/// <param name="markdown">Text with markdown.</param>
		/// <param name="media">Media object.</param>
		public static string FromMarkdown(string markdown, Media media = null)
		{
//			markdown.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br>");

			var html = markdownConverter.Transform(markdown);

			// Add style to html code
			#if __IOS__
			html = "<style>" + DefaultStyle + "</style>" + html;

			// Replace default values
			html = html.Replace("-Font0-", ((int)App.Fonts.Normal.FontSize * 0.9).ToString());
			html = html.Replace("-Font1-", ((int)App.Fonts.Normal.FontSize * 1.0).ToString());
			html = html.Replace("-Font2-", ((int)App.Fonts.Normal.FontSize * 1.2).ToString());
			html = html.Replace("-Font3-", ((int)App.Fonts.Normal.FontSize * 1.4).ToString());
			html = html.Replace("-Font4-", ((int)App.Fonts.Normal.FontSize * 1.8).ToString());
			html = html.Replace("-Font5-", ((int)App.Fonts.Normal.FontSize * 2.0).ToString());
			html = html.Replace("-TextAlign-", AlignmentToString(App.Prefs.TextAlignment));
			html = html.Replace("-TextColor-", ColorToHTML(App.Colors.Text));
			html = html.Replace("-BackgroundColor-", ColorToHTML(App.Colors.Background));
			#endif

			return html;
		}

		/// <summary>
		/// Froms the html.
		/// </summary>
		/// <returns>HTML with replaced medias.</returns>
		/// <param name="html">Html text.</param>
		public static string FromHtml(string html)
		{
			string result;

			// TODO
			// Add engine, so it is possible to retrive medias by name

			// TODO
			// Remove <head></head> if there is one in the code
			// Replace all "Wherigo://ZMedia" with the base64 representation of this media
			throw new NotImplementedException("Html isn't supported yet");

			return result;
		}

		#region Private Functions

		/// <summary>
		/// Converts the string to HTM and add decoration.
		/// </summary>
		/// <returns>String converted and docorated as HTML.</returns>
		/// <param name="text">Text to convert.</param>
		/// <param name="media">Media object.</param>
		public static string ConvertStringToHTML(string text, Media media = null)
		{
			var code = new StringBuilder();
			return text;
			code.Append("<html>");
			code.Append("<head>");
			code.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, miniumum-scale=0.5, maximum-scale=40.0\"/>");
			code.Append("<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">");
			code.Append("<style>");
			code.Append("html {");
			code.AppendFormat("	background-color: #{0};", ColorToHTML(App.Colors.Background));
			code.Append("	padding: 2px;");
			code.Append("	margin: 0px;");
			code.Append("}");
			code.Append("body {");
			code.AppendFormat("	color: #{0};", ColorToHTML(App.Colors.Text));
			code.AppendFormat("	background-color: #{0};", ColorToHTML(App.Colors.Background));
			code.Append("	font-family: sans-serif;");
			code.AppendFormat("	font-size: {0}px;", App.Prefs.TextSize);
			code.Append("}");
			code.Append("img{");

			switch (App.Prefs.ImageResize)
			{
				case ImageResize.NoResize:
					break;
				case ImageResize.ShrinkWidth:
					code.Append("	max-width:100%;");
					break;
				case ImageResize.ResizeWidth:
					code.Append("	width:100%;");
					break;
				case ImageResize.ResizeHeight:
					code.Append("	max-height:50%;");
					break;
			}

			code.Append("}");
			code.Append("</style>");
			code.Append("</head>");
			code.Append("<body>");

			// If there is a media, insert it
			if (media != null)
			{
				code.AppendFormat("<div align=\"{0}\">", AlignmentToString(App.Prefs.ImageAlignment));
				code.Append("<img src=\"data:;base64," + System.Convert.ToBase64String(media.Data) + "\">");
				code.Append("</div>");
			}

			// Are media and text visible, than separate with a newline
			if (media != null && !string.IsNullOrEmpty(text))
			{
				code.Append("<br>");
			}

			// If there is text, insert it
			if (!string.IsNullOrEmpty(text))
			{
				code.AppendFormat("<div align=\"{0}\">", AlignmentToString(App.Prefs.TextAlignment));
				code.Append(text.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br>"));
			}

			code.Append("</div>");
			code.Append("</body>");
			code.Append("</html>");

			return code.ToString();
		}

		/// <summary>
		/// Convert alignments to string.
		/// </summary>
		/// <returns>Alignment as HTML string.</returns>
		/// <param name="align">Align of text or image.</param>
		private static string AlignmentToString(TextAlignment align)
		{
			string result = string.Empty;

			switch (align)
			{
				case TextAlignment.Start:
					result = "left";
					break;
				case TextAlignment.Center:
					result = "center";
					break;
				case TextAlignment.End:
					result = "right";
					break;
			}

			return result;
		}

		/// <summary>
		/// Convert Color to HTML.
		/// </summary>
		/// <returns>Color in HTML format.</returns>
		/// <param name="color">Color as Color.</param>
		private static string ColorToHTML(Color color)
		{
			return string.Format("{0:X2}{1:X2}{2:X2}", (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
		}

		#endregion
	}
}
