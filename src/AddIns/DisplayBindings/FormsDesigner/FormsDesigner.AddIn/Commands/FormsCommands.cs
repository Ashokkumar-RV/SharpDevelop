﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using ICSharpCode.Core;
using ICSharpCode.Core.WinForms;
using ICSharpCode.FormsDesigner.Services;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;

namespace ICSharpCode.FormsDesigner.Commands
{
	/// <summary>
	/// This is the base class for all designer menu commands
	/// </summary>
	public abstract class AbstractFormsDesignerCommand : AbstractMenuCommand
	{
		public abstract CommandIDEnum CommandID {
			get;
		}
		
		protected virtual bool CanExecuteCommand(FormsDesignerManager host)
		{
			return true;
		}
		
		protected FormsDesignerViewContent FormsDesigner {
			get {
				IWorkbenchWindow window = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
				if (window == null) {
					return null;
				}
				return window.ActiveViewContent as FormsDesignerViewContent;
			}
		}
		public override void Run()
		{
			try {
				FormsDesignerViewContent formsDesigner = FormsDesigner;
				if (formsDesigner != null && CanExecuteCommand(formsDesigner.AppDomainManager)) {
					IMenuCommandServiceProxy menuCommandService = (IMenuCommandServiceProxy)formsDesigner.AppDomainManager.MenuCommandService;
					menuCommandService.GlobalInvoke(CommandID);
				}
			} catch (Exception e) {
				MessageService.ShowException(e);
			}
		}

		internal virtual void CommandCallBack(object sender, EventArgs e)
		{
			this.Run();
		}
	}

	public class ViewCode : AbstractMenuCommand
	{
		FormsDesignerViewContent FormDesigner {
			get {
				IWorkbenchWindow window = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
				if (window == null) {
					return null;
				}
				return window.ActiveViewContent as FormsDesignerViewContent;
			}
		}
		
		public override void Run()
		{
			IWorkbenchWindow window = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
			if (window == null) {
				return;
			}
			
			FormsDesignerViewContent formDesigner = FormDesigner;
			if (formDesigner != null) {
				formDesigner.ShowSourceCode();
			}
		}
	}

	public class ShowProperties : AbstractMenuCommand
	{
		public override void Run()
		{
			PadDescriptor padContent = WorkbenchSingleton.Workbench.GetPad(typeof(ICSharpCode.SharpDevelop.Gui.PropertyPad));
			if (padContent != null) {
				padContent.BringPadToFront();
			}
		}
	}
	
	public class DesignerVerbSubmenuBuilder : ISubmenuBuilder
	{
		public ToolStripItem[] BuildSubmenu(Codon codon, object owner)
		{
			List<ToolStripItem> items = new List<ToolStripItem>();
			
			foreach (IDesignerVerbProxy verb in ((FormsDesignerManager)owner).CommandVerbs) {
				items.Add(new ContextMenuCommand(verb));
			}
			
			// add separator at the end of custom designer verbs
			if (items.Count > 0) {
				items.Add(new MenuSeparator());
			}
			
			return items.ToArray();
		}
		
		class ContextMenuCommand : ICSharpCode.Core.WinForms.MenuCommand
		{
			IDesignerVerbProxy verb;
			
			public ContextMenuCommand(IDesignerVerbProxy verb) : base(verb.Text)
			{
				this.Enabled = verb.Enabled;
//				this.Checked = verb.Checked;
				
				this.verb = verb;
				Click += new EventHandler(InvokeCommand);
			}
			
			void InvokeCommand(object sender, EventArgs e)
			{
				try {
					verb.Invoke();
				} catch (Exception ex) {
					MessageService.ShowException(ex);
				}
			}
		}
	}
	
	#region Align Commands
	public class AlignToGrid : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.AlignToGrid;
			}
		}
	}
	
	public class AlignLeft : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.AlignLeft;
			}
		}
	}
	
	public class AlignRight : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.AlignRight;
			}
		}
	}
	
	public class AlignTop : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.AlignTop;
			}
		}
	}
	
	public class AlignBottom : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.AlignBottom;
			}
		}
	}
	
	public class AlignHorizontalCenters : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.AlignHorizontalCenters;
			}
		}
	}
	
	public class AlignVerticalCenters : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.AlignVerticalCenters;
			}
		}
	}
	#endregion

	#region Make Same Size Commands
	public class SizeToGrid : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.SizeToGrid;
			}
		}
	}
	
	public class SizeToControl : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.SizeToControl;
			}
		}
	}
	
	public class SizeToControlHeight : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.SizeToControlHeight;
			}
		}
	}
	
	public class SizeToControlWidth : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.SizeToControlWidth;
			}
		}
	}
	#endregion

	#region Horizontal Spacing Commands
	public class HorizSpaceMakeEqual : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.HorizSpaceMakeEqual;
			}
		}
		
		protected override bool CanExecuteCommand(FormsDesignerManager host)
		{
			ISelectionService selectionService = (ISelectionService)host.GetService(typeof(ISelectionService));
			return selectionService.SelectionCount > 1;
		}
	}
	
	public class HorizSpaceIncrease : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.HorizSpaceIncrease;
			}
		}
	}
	
	public class HorizSpaceDecrease : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.HorizSpaceDecrease;
			}
		}
	}
	
	public class HorizSpaceConcatenate : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.HorizSpaceConcatenate;
			}
		}
	}
	#endregion
	
	#region Vertical Spacing Commands
	public class VertSpaceMakeEqual : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.VertSpaceMakeEqual;
			}
		}
		
		protected override bool CanExecuteCommand(FormsDesignerManager host)
		{
			ISelectionService selectionService = (ISelectionService)host.GetService(typeof(ISelectionService));
			return selectionService.SelectionCount > 1;
		}
		
	}
	
	public class VertSpaceIncrease : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.VertSpaceIncrease;
			}
		}
	}
	
	public class VertSpaceDecrease : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.VertSpaceDecrease;
			}
		}
	}
	
	public class VertSpaceConcatenate : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.VertSpaceConcatenate;
			}
		}
	}
	#endregion

	#region Center Commands
	public class CenterHorizontally : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.CenterHorizontally;
			}
		}
	}
	public class CenterVertically : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.CenterVertically;
			}
		}
	}
	#endregion
	
	#region Order Commands
	public class SendToBack : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.SendToBack;
			}
		}
	}
	
	public class BringToFront : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.BringToFront;
			}
		}
	}
	#endregion

	#region Tray Commands
	
	public class LineUpIcons : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.LineupIcons;
			}
		}
	}
	
	public class ShowLargeIcons : AbstractCheckableMenuCommand
	{
		FormsDesignerViewContent FormDesigner {
			get {
				IWorkbenchWindow window = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
				if (window == null) {
					return null;
				}
				return window.ActiveViewContent as FormsDesignerViewContent;
			}
		}
		public override bool IsChecked {
			get {
				ComponentTray tray = Tray;
				if (tray != null) {
					return tray.ShowLargeIcons;
				}
				return false;
			}
			set {
				ComponentTray tray = Tray;
				if (tray != null) {
					tray.ShowLargeIcons = value;
				}
			}
		}
		ComponentTray Tray {
			get {
				FormsDesignerViewContent formDesigner = FormDesigner;
				if (formDesigner != null) {
					return formDesigner.AppDomainManager.GetService(typeof(ComponentTray)) as ComponentTray;
				}
				return null;
				
			}
		}
		public override void Run()
		{
		}
	}
	#endregion

	#region Global Commands
	public class LockControls : AbstractFormsDesignerCommand
	{
		public override CommandIDEnum CommandID {
			get {
				return CommandIDEnum.LockControls;
			}
		}
	}
	
	/// <summary>
	/// Displays the tab order mode.
	/// </summary>
	public class ViewTabOrder : AbstractCheckableMenuCommand
	{
		public override bool IsChecked {
			get {
				FormsDesignerViewContent formDesigner = FormDesigner;
				if (formDesigner != null) {
					return formDesigner.IsTabOrderMode;
				}
				return false;
			}
			set {
				SetTabOrder(value);
			}
		}
		FormsDesignerViewContent FormDesigner {
			get {
				IWorkbenchWindow window = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
				if (window == null) {
					return null;
				}
				return window.ActiveViewContent as FormsDesignerViewContent;
			}
		}
		
		void SetTabOrder(bool show)
		{
			FormsDesignerViewContent formDesigner = FormDesigner;
			if (formDesigner != null) {
				if (show) {
					formDesigner.ShowTabOrder();
				} else {
					formDesigner.HideTabOrder();
				}
			}
		}
	}
	#endregion
}
