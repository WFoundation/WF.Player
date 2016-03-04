// <copyright file="ExtendedLabel.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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
using WF.Player.Services.Settings;

namespace WF.Player.Controls
{
	using System;
	using Xamarin.Forms;

	/// <summary>
	/// Label, capable of displaying text with html tags.
	/// </summary>
	public class ExtendedLabel : Label
	{
		#region Properties

		#region TextExt

		/// <summary>
		/// Gets the extended text (html code).
		/// </summary>
		/// <value>The html code.</value>
		public string TextExt
		{
			get
			{
				string html;

				if (UseMarkdown)
				{
					html = ConverterToHtml.FromMarkdown(Text, HorizontalTextAlignment, null, (int)FontSize);
				}
				else
				{
					html = ConverterToHtml.FromText(Text, HorizontalTextAlignment, null, (int)FontSize);
				}

				while (html.EndsWith(Environment.NewLine))
				{
					html = html.Substring(0, html.Length - 1);
				}

				return html;
			}
		}

		#endregion

		#region UseMarkdown

		/// <summary>
		/// Gets a value indicating whether this <see cref="WF.Player.Controls.ExtendedLabel"/> use markdown.
		/// </summary>
		/// <value><c>true</c> if use markdown; otherwise, <c>false</c>.</value>
		public bool UseMarkdown
		{
			get;
			set;
		}

		#endregion

		#endregion
	}
}
