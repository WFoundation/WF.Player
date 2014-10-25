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
	using WF.Player.Core.Engines;
	using Xamarin.Forms;

	/// <summary>
	/// Convert to html.
	/// </summary>
	public class ConverterToHtml
	{
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
			markdown.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br>");

			return ConvertStringToHTML(new MarkdownSharp.Markdown().Transform(markdown), media);
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
