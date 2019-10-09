/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GitHubViewer.Extensions
{
    internal static class ExtensionHelper
    {
        #region Public methods

        public static readonly String ValueUnset = "Unset";

        public static readonly Thickness LabelPadding = new Thickness(5, 0, 5, 5);

        public static readonly Thickness ValuePadding = new Thickness(0, 0, 0, 5);

        public static String ToDisplayValue(this Object value)
        {
            if (value == null)
            {
                return ExtensionHelper.ValueUnset;
            }

            if (value is String)
            {
                return (value as String).ToDisplayValue();
            }

            if (value is Boolean)
            {
                return ((Boolean)value).ToDisplayValue();
            }

            if (value is DateTime)
            {
                return ((DateTime)value).ToDisplayValue();
            }

            if (value is IEnumerable)
            {
                return (value as IEnumerable).ToDisplayValue();
            }

            return value.ToString();
        }

        public static String ToDisplayLabel(this String label)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                return ExtensionHelper.ValueUnset;
            }

            // Insert one space right before each capital letter...
            return $"{Regex.Replace(label, "([a-z])([A-Z])", "$1 $2")}:";
        }

        public static String ToDisplayValue(this String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return ExtensionHelper.ValueUnset;
            }

            return value as String;
        }

        public static String ToDisplayValue(this Boolean value)
        {
            return value ? "Yes" : "No";
        }

        public static String ToDisplayValue(this DateTime value)
        {
            if (value == null)
            {
                return ExtensionHelper.ValueUnset;
            }

            if (value.Kind == DateTimeKind.Utc)
            {
                return value.ToString("yyyy-MM-dd HH:mm:ss (UTC)");
            }
            else
            {
                return value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public static String ToDisplayValue(this IEnumerable value)
        {
            if (value == null)
            {
                return ExtensionHelper.ValueUnset;
            }

            IEnumerable<Object> result = (value as IEnumerable).Cast<Object>();
            return result.Any() ? String.Join(", ", result) : ExtensionHelper.ValueUnset;
        }

        public static UIElement ToLabelElement(this String label)
        {
            return ExtensionHelper.ToLabelElement(label, ExtensionHelper.ValuePadding);
        }

        public static UIElement ToLabelElement(this String label, Thickness padding)
        {
            return new Label
            {
                Padding = padding,
                BorderThickness = new Thickness(0),
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = label.ToDisplayLabel(),
            };
        }

        public static UIElement ToValueElement(this Object value)
        {
            return ExtensionHelper.ToValueElement(value, ExtensionHelper.ValuePadding);
        }

        public static UIElement ToValueElement(this Object value, Thickness padding)
        {
            return value.ToValueElement(padding, null);
        }

        public static UIElement ToValueElement(this Object value, Thickness padding, String tooltip)
        {
            return new TextBox
            {
                Padding = padding,
                IsReadOnly = true,
                Cursor = Cursors.Arrow,
                Background = Brushes.Transparent,
                TextWrapping = TextWrapping.Wrap,
                BorderThickness = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = value.ToDisplayValue(),
                ToolTip = (String.IsNullOrWhiteSpace(tooltip) ? null : tooltip),
            };
        }

        #endregion
    }
}
