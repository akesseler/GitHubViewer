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
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace GitHubViewer.Extensions
{
    internal static class LicenseExtension
    {
        #region Public methods

        public static UIElement ToElement(this ILicense license)
        {
            if (license == null)
            {
                return new Label() { Content = ExtensionHelper.ValueUnset };
            }

            Expander expander = new Expander()
            {
                Header = license.Name.ToDisplayValue(),
                Template = Application.Current.Resources["NiceExpanderTemplate"] as ControlTemplate,
            };

            PropertyInfo[] properties = typeof(ILicense).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties == null || !properties.Any())
            {
                return new UIElement();
            }

            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            Int32 rows = properties.Length;

            for (Int32 row = 0; row < rows; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                PropertyInfo property = properties[row];

                Label label = license.ToLabel(property);
                grid.Children.Add(label);
                Grid.SetRow(label, row);
                Grid.SetColumn(label, 0);

                UIElement value = license.ToValue(property);
                grid.Children.Add(value);
                Grid.SetRow(value, row);
                Grid.SetColumn(value, 1);
            }

            expander.Content = grid;

            return expander;
        }

        public static Label ToLabel(this ILicense license, PropertyInfo property)
        {
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license));
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

        public static UIElement ToValue(this ILicense license, PropertyInfo property)
        {
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Object value = property.GetValue(license);

            return value.ToValueElement(ExtensionHelper.ValuePadding);
        }

        #endregion
    }
}
