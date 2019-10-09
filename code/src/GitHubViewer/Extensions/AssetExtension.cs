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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GitHubViewer.Extensions
{
    internal static class AssetExtension
    {
        #region Public methods

        public static UIElement ToElement(this IEnumerable<IAsset> assets)
        {
            if (assets == null || !assets.Any())
            {
                return new Label() { Content = ExtensionHelper.ValueUnset };
            }

            Grid container = new Grid
            {
                Background = Application.Current.Resources["DefaultControlBrush"] as SolidColorBrush,
            };

            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });

            Int32 row = 0;

            foreach (IAsset asset in assets)
            {
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                UIElement element = asset.ToElement();

                container.Children.Add(element);

                Grid.SetRow(element, row);
                Grid.SetColumn(element, 0);

                row++;
            }

            return container;
        }

        public static UIElement ToElement(this IAsset asset)
        {
            if (asset == null)
            {
                return new Label() { Content = ExtensionHelper.ValueUnset };
            }

            Expander expander = new Expander()
            {
                Header = $"{asset.Name.ToDisplayValue()} ({asset.DownloadCount.ToDisplayValue()})",
                Template = Application.Current.Resources["NiceExpanderTemplate"] as ControlTemplate,
            };

            PropertyInfo[] properties = typeof(IAsset).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties == null || !properties.Any())
            {
                return new UIElement();
            }

            Grid container = new Grid();

            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });

            Int32 rows = properties.Length;

            for (Int32 row = 0; row < rows; row++)
            {
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                PropertyInfo property = properties[row];

                Label label = asset.ToLabel(property);
                container.Children.Add(label);
                Grid.SetRow(label, row);
                Grid.SetColumn(label, 0);

                UIElement element = asset.ToValue(property);
                container.Children.Add(element);
                Grid.SetRow(element, row);
                Grid.SetColumn(element, 1);
            }

            expander.Content = container;

            return expander;
        }

        public static Label ToLabel(this IAsset asset, PropertyInfo property)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
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

        public static UIElement ToValue(this IAsset asset, PropertyInfo property)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Object value = property.GetValue(asset);

            if (value is IOwner)
            {
                return (value as IOwner).ToElement();
            }

            return value.ToValueElement(ExtensionHelper.ValuePadding);
        }

        #endregion
    }
}
