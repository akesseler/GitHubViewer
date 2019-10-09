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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace GitHubViewer.Controls
{
    // http://andyonwpf.blogspot.com/2006/10/dropdownbuttons-in-wpf.html

    public class DropDownButton : ToggleButton
    {
        public static readonly DependencyProperty DropDownProperty =
            DependencyProperty.Register(
                "DropDown",
                typeof(ContextMenu),
                typeof(DropDownButton),
                new UIPropertyMetadata(null));

        public DropDownButton()
        {
            Binding binding = new Binding("DropDown.IsOpen");
            binding.Source = this;
            base.SetBinding(ToggleButton.IsCheckedProperty, binding);
        }

        public ContextMenu DropDown
        {
            get
            {
                return (ContextMenu)base.GetValue(DropDownButton.DropDownProperty);
            }
            set
            {
                base.SetValue(DropDownButton.DropDownProperty, value);
            }
        }

        protected override void OnClick()
        {
            if (this.DropDown != null)
            {
                this.DropDown.PlacementTarget = this;
                this.DropDown.Placement = PlacementMode.Bottom;
                this.DropDown.IsOpen = true;
            }
        }
    }
}
