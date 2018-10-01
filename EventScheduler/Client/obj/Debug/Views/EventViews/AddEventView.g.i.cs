#pragma checksum "..\..\..\..\Views\EventViews\AddEventView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B0182B608E38611ED36311C42508142FFB7C5DAE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Client.Converters;
using Client.Views.EventViews;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Client.Views.EventViews
{


    /// <summary>
    /// AddEventView
    /// </summary>
    public partial class AddEventView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden


#line 42 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EventTitle;

#line default
#line hidden


#line 43 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Description;

#line default
#line hidden


#line 45 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker ScheduledDateTimeBeging;

#line default
#line hidden


#line 46 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker ScheduledDateTimeEnd;

#line default
#line hidden


#line 48 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBar AvailablePeople;

#line default
#line hidden


#line 51 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddParticipantButton;

#line default
#line hidden


#line 60 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView AvailablePeopleList;

#line default
#line hidden


#line 74 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBar Participatns;

#line default
#line hidden


#line 77 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveParticipantButton;

#line default
#line hidden


#line 86 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ParticipatnsList;

#line default
#line hidden


#line 101 "..\..\..\..\Views\EventViews\AddEventView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addButton;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Client;component/views/eventviews/addeventview.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\Views\EventViews\AddEventView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.AddEventUserControl = ((Client.Views.EventViews.AddEventView)(target));
                    return;
                case 2:
                    this.EventTitle = ((System.Windows.Controls.TextBox)(target));
                    return;
                case 3:
                    this.Description = ((System.Windows.Controls.TextBox)(target));
                    return;
                case 4:
                    this.ScheduledDateTimeBeging = ((System.Windows.Controls.DatePicker)(target));
                    return;
                case 5:
                    this.ScheduledDateTimeEnd = ((System.Windows.Controls.DatePicker)(target));
                    return;
                case 6:
                    this.AvailablePeople = ((System.Windows.Controls.Primitives.StatusBar)(target));
                    return;
                case 7:
                    this.AddParticipantButton = ((System.Windows.Controls.Button)(target));
                    return;
                case 8:
                    this.AvailablePeopleList = ((System.Windows.Controls.ListView)(target));
                    return;
                case 9:
                    this.Participatns = ((System.Windows.Controls.Primitives.StatusBar)(target));
                    return;
                case 10:
                    this.RemoveParticipantButton = ((System.Windows.Controls.Button)(target));
                    return;
                case 11:
                    this.ParticipatnsList = ((System.Windows.Controls.ListView)(target));
                    return;
                case 12:
                    this.addButton = ((System.Windows.Controls.Button)(target));
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.UserControl AddEventUserControl;
    }
}

