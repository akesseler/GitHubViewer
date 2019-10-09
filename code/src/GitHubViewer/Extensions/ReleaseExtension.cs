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

using Plexdata.GitHub.Accessor.Abstraction.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace GitHubViewer.Extensions
{
    internal static class ReleaseExtension
    {
        #region Public methods

        public static UIElement ToElement(this IRelease release)
        {
            if (release == null)
            {
                return new UIElement();
            }

            PropertyInfo[] properties = typeof(IRelease).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties == null || !properties.Any())
            {
                return new UIElement();
            }

            Grid result = new Grid();

            result.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            result.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });

            Int32 rows = properties.Length;

            for (Int32 row = 0; row < rows; row++)
            {
                result.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                PropertyInfo property = properties[row];

                Label label = release.ToLabel(property);
                result.Children.Add(label);
                Grid.SetRow(label, row);
                Grid.SetColumn(label, 0);

                UIElement value = release.ToValue(property);
                result.Children.Add(value);
                Grid.SetRow(value, row);
                Grid.SetColumn(value, 1);
            }

            return result;
        }

        public static Label ToLabel(this IRelease release, PropertyInfo property)
        {
            if (release == null)
            {
                throw new ArgumentNullException(nameof(release));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return new Label()
            {
                Padding = ExtensionHelper.LabelPadding,
                FontWeight = FontWeights.DemiBold,
                Content = property.Name.ToDisplayLabel(),
            };
        }

        public static UIElement ToValue(this IRelease release, PropertyInfo property)
        {
            if (release == null)
            {
                throw new ArgumentNullException(nameof(release));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Object value = property.GetValue(release);

            if (value is IOwner)
            {
                return (value as IOwner).ToElement();
            }

            if ((value as IEnumerable)?.GetType().GetGenericArguments().FirstOrDefault() == typeof(IAsset))
            {
                return (value as IEnumerable<IAsset>).ToElement();
            }

            return value.ToValueElement(ExtensionHelper.ValuePadding);
        }

        #endregion
    }
}
