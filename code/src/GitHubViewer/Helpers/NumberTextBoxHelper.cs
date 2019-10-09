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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GitHubViewer.Helpers
{
    public static class NumberTextBoxHelper
    {
        #region Private constant definitions

        private const String MinimumPropertyName = "Minimum";

        private const String MaximumPropertyName = "Maximum";

        private const String DefaultPropertyName = "Default";

        #endregion

        #region Public dependency properties

        public static DependencyProperty MinimumProperty =
            DependencyProperty.RegisterAttached(
                NumberTextBoxHelper.MinimumPropertyName,
                typeof(Int32?),
                typeof(NumberTextBoxHelper),
                new PropertyMetadata(null, NumberTextBoxHelper.OnDependencyPropertyChanged));

        public static DependencyProperty MaximumProperty =
            DependencyProperty.RegisterAttached(
                NumberTextBoxHelper.MaximumPropertyName,
                typeof(Int32?),
                typeof(NumberTextBoxHelper),
                new PropertyMetadata(null, NumberTextBoxHelper.OnDependencyPropertyChanged));

        public static DependencyProperty DefaultProperty =
            DependencyProperty.RegisterAttached(
                NumberTextBoxHelper.DefaultPropertyName,
                typeof(Int32?),
                typeof(NumberTextBoxHelper),
                new PropertyMetadata(null, NumberTextBoxHelper.OnDependencyPropertyChanged));

        #endregion

        #region Public dependency properties setter and getter

        public static void SetMinimum(TextBox source, Int32? value)
        {
            source.SetValue(NumberTextBoxHelper.MinimumProperty, value);
        }

        public static Int32? GetMinimum(TextBox source)
        {
            return (Int32?)source.GetValue(NumberTextBoxHelper.MinimumProperty);
        }

        public static void SetMaximum(TextBox source, Int32? value)
        {
            source.SetValue(NumberTextBoxHelper.MaximumProperty, value);
        }

        public static Int32? GetMaximum(TextBox source)
        {
            return (Int32?)source.GetValue(NumberTextBoxHelper.MaximumProperty);
        }

        public static void SetDefault(TextBox source, Int32? value)
        {
            source.SetValue(NumberTextBoxHelper.DefaultProperty, value);
        }

        public static Int32? GetDefault(TextBox source)
        {
            return (Int32?)source.GetValue(NumberTextBoxHelper.DefaultProperty);
        }

        #endregion

        #region Private event handlers

        private static void OnDependencyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (!(sender is TextBox source))
            {
                throw new ArgumentException("Attached property must be used with TextBox.", nameof(sender));
            }

            switch (args.Property.Name)
            {
                case NumberTextBoxHelper.MinimumPropertyName:
                    {
                        Int32? value = (Int32?)args.NewValue;

                        if (value.HasValue)
                        {
                            source.PreviewTextInput += OnTextBoxPreviewTextInput;
                            DataObject.AddPastingHandler(source, OnTextBoxPasteEventHandler);
                        }
                        else
                        {
                            source.PreviewTextInput -= OnTextBoxPreviewTextInput;
                            DataObject.RemovePastingHandler(source, OnTextBoxPasteEventHandler);
                        }
                    }
                    break;

                case NumberTextBoxHelper.MaximumPropertyName:
                    {
                        Int32? value = (Int32?)args.NewValue;

                        if (value.HasValue)
                        {
                            source.PreviewTextInput += OnTextBoxPreviewTextInput;
                            DataObject.AddPastingHandler(source, OnTextBoxPasteEventHandler);
                        }
                        else
                        {
                            source.PreviewTextInput -= OnTextBoxPreviewTextInput;
                            DataObject.RemovePastingHandler(source, OnTextBoxPasteEventHandler);
                        }
                    }
                    break;

                case NumberTextBoxHelper.DefaultPropertyName:
                    {
                        Int32? value = (Int32?)args.NewValue;

                        if (value.HasValue)
                        {
                            source.TextChanged += OnTextBoxTextChanged;
                        }
                        else
                        {
                            source.TextChanged -= OnTextBoxTextChanged;
                        }
                    }
                    break;
            }
        }

        private static void OnTextBoxPreviewTextInput(Object sender, TextCompositionEventArgs args)
        {
            String value;
            TextBox source = (TextBox)sender;

            if (source.SelectionLength == 0)
            {
                value = source.Text
                    .Insert(source.SelectionStart, args.Text);
            }
            else
            {
                value = source.Text
                    .Remove(source.SelectionStart, source.SelectionLength)
                    .Insert(source.SelectionStart, args.Text);
            }

            args.Handled = !NumberTextBoxHelper.IsValid(source, value);
        }

        private static void OnTextBoxPasteEventHandler(Object sender, DataObjectPastingEventArgs args)
        {
            String value;
            TextBox source = (TextBox)sender;

            if (!args.DataObject.GetDataPresent(typeof(String)))
            {
                args.CancelCommand();
                return;
            }

            String clipboard = (String)args.DataObject.GetData(typeof(String));

            if (String.IsNullOrWhiteSpace(clipboard))
            {
                args.CancelCommand();
                return;
            }

            if (source.SelectionLength == 0)
            {
                value = source.Text
                    .Insert(source.SelectionStart, clipboard);
            }
            else
            {
                value = source.Text
                    .Remove(source.SelectionStart, source.SelectionLength)
                    .Insert(source.SelectionStart, clipboard);
            }

            if (!NumberTextBoxHelper.IsValid(source, value))
            {
                args.CancelCommand();
                return;
            }
        }

        private static void OnTextBoxTextChanged(Object sender, TextChangedEventArgs args)
        {
            TextBox source = (TextBox)sender;
            String value = source.Text;

            if (String.IsNullOrWhiteSpace(value) || !NumberTextBoxHelper.IsValid(source, value))
            {
                source.Text = NumberTextBoxHelper.GetDefault(source)?.ToString();
            }
        }

        #endregion

        #region Private methods

        private static Boolean IsValid(TextBox source, String value)
        {
            if (value == null)
            {
                return false;
            }

            Int32? minimum = NumberTextBoxHelper.GetMinimum(source);
            Int32? maximum = NumberTextBoxHelper.GetMaximum(source);

            if (value.Length == 1 && value[0] == '-')
            {
                if (minimum.HasValue && minimum.Value >= 1)
                {
                    return false;
                }

                return true;
            }

            if (!Int32.TryParse(value, out Int32 number))
            {
                return false;
            }

            if (minimum.HasValue && number < minimum.Value)
            {
                return false;
            }

            if (maximum.HasValue && number > maximum.Value)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
