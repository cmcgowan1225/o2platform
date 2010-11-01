// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Text;
using O2.Interfaces.O2Core;
using O2.Interfaces.O2Findings;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.O2Findings;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.H2Scripts;
using O2.Views.ASCX;
using O2.Views.ASCX.CoreControls;
using O2.Views.ASCX.classes.MainGUI;
using O2.Views.ASCX.ExtensionMethods;
using O2.External.SharpDevelop.AST;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
using O2.API.AST.CSharp;
using O2.API.AST.ExtensionMethods;
using O2.API.AST.ExtensionMethods.CSharp;

using ICSharpCode.TextEditor;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast; 
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using System.CodeDom;
using O2.Views.ASCX.O2Findings;
using O2.Views.ASCX.DataViewers;
using System.Security.Cryptography;

//O2Ref:O2_API_AST.dll

namespace O2.XRules.Database.Utils
{
	public static class ExtraMethodsToAddToO2CodeBase_IO
	{
		// Config files (can't easily put this on the main
        public static Panel editLocalConfigFile(this string file)
        {
            var panel = O2Gui.open<Panel>("Editing local config file: {0}".format(file), 700, 300);
            return file.editLocalConfigFile(panel);
        }
		
		// new one
		public static T resizeFormToControlSize<T>(this T control, Control controlToSync)
			where T : Control
		{
			if (controlToSync.notNull())
			{
				var parentForm = control.parentForm();
				if (parentForm.notNull())
				{
					var top = controlToSync.PointToScreen(System.Drawing.Point.Empty).Y;
					var left = controlToSync.PointToScreen(System.Drawing.Point.Empty).X;
					var width = controlToSync.Width;
					var height = controlToSync.Height;
					"Setting parentForm location to {0}x{1} : {2}x{3}".info(top, left, width, height);
					parentForm.Top = top;
					parentForm.Left = left;
					parentForm.Width = width;
					parentForm.Height = height;
				}
			}
			return control;
		}
		
		public static string saveImageFromClipboardToFile(this object _object)
		{
			var clipboardImagePath = _object.saveImageFromClipboard();
			if (clipboardImagePath.fileExists())
			{
				var fileToSave = O2Forms.askUserForFileToSave(PublicDI.config.O2TempDir,"*.jpg");
				if (fileToSave.valid())
				{
					Files.MoveFile(clipboardImagePath, fileToSave);
					"Clipboard Image saved to: {0}".info(fileToSave);
				}
			}
			return clipboardImagePath;
		}
		
		
		//Split Container
		
		public static SplitContainer splitContainer(this Control control)
		{
			return control.parent<SplitContainer>();
		}
		
		public static SplitContainer splitterWidth(this SplitContainer splitContainer, int value)
		{
			splitContainer.invokeOnThread(()=> splitContainer.SplitterWidth = value);
			return splitContainer;
		}
		
		
		//Label

		public static Label autoSize(this Label label, bool value)
		{
			label.invokeOnThread(
				()=>{						
						label.AutoSize = value;
					});
			return label;
		}
		
		public static Label text_Center(this Label label)			
		{			
			label.invokeOnThread(
				()=>{						
						label.autoSize(false);
						label.TextAlign = ContentAlignment.MiddleCenter;
					});
			return label;
		}				
		//Control (Font)			
		
		
		public static T size<T>(this T control, int value)
			where T : Control
		{
			return control.textSize(value);
		}
		
		public static T size<T>(this T control, string value)
			where T : Control
		{
			return control.textSize(value.toInt());
		}
		
		public static T font<T>(this T control, string fontFamily, string size)
			where T : Control
		{
			return control.font(fontFamily, size.toInt());
		}
		
		public static T font<T>(this T control, string fontFamily, int size)
			where T : Control
		{
			return control.font(new FontFamily(fontFamily), size);
		}
		
		public static T font<T>(this T control, FontFamily fontFamily, string size)
			where T : Control
		{
			return control.font(fontFamily, size.toInt());
		}
		
		public static T font<T>(this T control, FontFamily fontFamily, int size)
			where T : Control
		{
			if (control.isNull())
				return null;
			control.invokeOnThread(
				()=>{
						if (fontFamily.isNull())
							fontFamily = control.Font.FontFamily;
						control.Font = new Font(fontFamily, size);
					});
			return control;
		}
		
		public static T font<T>(this T control, string fontFamily)
			where T : Control
		{
			return control.fontFamily(fontFamily);
		}
		
		public static T fontFamily<T>(this T control, string fontFamily)
			where T : Control
		{
			control.invokeOnThread(
				()=> control.Font = new Font(new FontFamily(fontFamily), control.Font.Size));			
			return control;
		}
		
		public static T textSize<T>(this T control, int value)
			where T : Control
		{
			control.invokeOnThread(
				()=> control.Font = new Font(control.Font.FontFamily, value));			
			return control;
		}
		
		public static T font_bold<T>(this T control)		// just bold() conficts with WPF version
			where T : Control
		{
			control.invokeOnThread(
				()=> control.Font = new Font( control.Font, control.Font.Style | FontStyle.Bold ));
			return control;
		}
		
		public static T font_italic<T>(this T control)
			where T : Control
		{
			control.invokeOnThread(
				()=> control.Font = new Font( control.Font, control.Font.Style | FontStyle.Italic ));
			return control;
		}
		
		//ListBox
		
		public static ListBox add_ListBox(this Control control)
		{
			return control.add_Control<ListBox>();
		}
		
		public static ListBox add_Item(this ListBox listBox, object item)
		{
			return listBox.add_Items(item);
		}
		
		public static ListBox add_Items(this ListBox listBox, params object[] items)
		{
			return (ListBox)listBox.invokeOnThread(
				()=>{
						listBox.Items.AddRange(items);
						return listBox;
					});					
		}
		
		public static object selectedItem(this ListBox listBox)
		{
			return (object)listBox.invokeOnThread(
				()=>{	
						return listBox.SelectedItem;	
					});
		}
		
		public static T selectedItem<T>(this ListBox listBox)
		{			
			var selectedItem = listBox.selectedItem();
			if (selectedItem is T) 
				return (T) selectedItem;
			return default(T);					
		}
		
		public static ListBox select(this ListBox listBox, int selectedIndex)
		{
			return (ListBox)listBox.invokeOnThread(
				()=>{
						if (listBox.Items.size() > selectedIndex)
							listBox.SelectedIndex = selectedIndex;
						return listBox;
					});					
		}
		
		public static ListBox selectFirst(this ListBox listBox)
		{
			return listBox.select(0);
		}
		
		// ALREADY ADDED****
		//DateTime extensionMethods
		/*
		public static string safeFileName(this DateTime dateTime)
		{
			return Files.getSafeFileNameString(dateTime.str()); 
		}
		
		// ASCX TextBox
		
		public static TextBox allowTabs(this TextBox textBox)
		{
			return textBox.acceptsTab();
		}
		public static TextBox acceptsTab(this TextBox textBox)
		{
			return textBox.acceptsTab(true);
		}
		public static TextBox acceptsTab(this TextBox textBox, bool value)
		{
			textBox.invokeOnThread(()=> textBox.AcceptsTab = value);
			return textBox;
		}
		// Collections Dictionary<string,string>
		
		
		//Processes ExtensionMethods API				
		
		
	
		// Controls ExtensionMethods
		
		public static Form opacity(this Form form, double value)			
		{
			form.invokeOnThread(
				()=>{
						form.Opacity = value;
					});
			return form;
		}
		
		public static TextBox add_TextBox(this Control control, string labelText, string defaultTextBoxText)
		{
			return control.add_Label(labelText).top(3)
						  .append_TextBox(defaultTextBoxText)
						  .align_Right(control);;
		}
		
		public static TextBox add_TextBox(this Control control, int top, string labelText, string defaultTextBoxText)
		{
			return control.add_Label(labelText).top(top+3)
						  .append_TextBox(defaultTextBoxText)
						  .align_Right(control);;
		}
		
		public static CheckBox add_CheckBox(this Control control, int top, string checkBoxText)
		{
			return control.add_CheckBox(top, 0, checkBoxText);
		}
		
		public static CheckBox add_CheckBox(this Control control, int top,int left, string checkBoxText)
		{			
			return control.add_CheckBox(checkBoxText, top, left,(value)=> {})
						  .autoSize();
		}
		
		public static Button add_Button(this Control control, int top, string buttonText)
		{
			return control.add_Button(top, 0, buttonText);
		}
		
		public static Button add_Button(this Control control, int top,int left, string buttonText)
		{
			return control.add_Button(buttonText, top, left);
		}
		
		
		//PropertyGrid
		
		public static PropertyGrid toolBarVisible(this PropertyGrid propertyGrid, bool value)
		{
			propertyGrid.invokeOnThread(()=>propertyGrid.ToolbarVisible = value);
		
			return propertyGrid;
		}
		
		public static PropertyGrid helpVisible(this PropertyGrid propertyGrid, bool value)
		{
			propertyGrid.invokeOnThread(()=>propertyGrid.HelpVisible = value);		
			return propertyGrid;
		}
		
		public static PropertyGrid sort_Alphabetical(this PropertyGrid propertyGrid)
		{
			propertyGrid.invokeOnThread(()=>propertyGrid.PropertySort = PropertySort.Alphabetical);		
			return propertyGrid;
		}
		
		public static PropertyGrid sort_Categorized(this PropertyGrid propertyGrid)
		{
			propertyGrid.invokeOnThread(()=>propertyGrid.PropertySort = PropertySort.Categorized);		
			return propertyGrid;
		}
		
		public static PropertyGrid sort_CategorizedAlphabetical(this PropertyGrid propertyGrid)
		{
			propertyGrid.invokeOnThread(()=>propertyGrid.PropertySort = PropertySort.CategorizedAlphabetical);		
			return propertyGrid;
		}
		
		
		// ascx_Directory
		public static ascx_Directory processDroppedObjects(this ascx_Directory directory, bool value)
		{
			directory.invokeOnThread(()=>directory._ProcessDroppedObjects = value);
			return directory;
		}
		
		public static ascx_Directory handleDrop(this ascx_Directory directory, bool value)
		{
			directory.invokeOnThread(()=>directory._HandleDrop = value);
			return directory;
		}					
		
		// ascx_SourceCodeEditor ExtensionMethods
		public static ascx_SourceCodeEditor editScript(this string scriptOrFile)
		{
			if (scriptOrFile.fileExists().isFalse())
			{
				if (scriptOrFile.local().valid())				
					scriptOrFile= scriptOrFile.local();				
				else
				{					
					var h2Script = new H2(scriptOrFile);
					scriptOrFile = PublicDI.config.getTempFileInTempDirectory(".h2");
					h2Script.save(scriptOrFile);
				}
			}			
			return O2Gui.open<Panel>(scriptOrFile.fileName(),800,400)
			     		.add_SourceCodeEditor()
			     		.open(scriptOrFile);
		}
						
		// Objects
		
		public static Form lastFormLoaded(this string dummyString)
		{
			return dummyString.lastWindowShown();
		}
		public static Form lastWindowShown(this string dummyString)
		{
			return dummyString.applicationWinForms().Last();
		}
		
		public static Exception log(this Exception ex)
		{
			ex.log("");
			return ex;
		}
		
		// ascx_TableList in O2.Views.ASCX.DataViewers
		public static ascx_TableList title(this ascx_TableList tableList, string title)
		{
			tableList.invokeOnThread(()=> tableList._Title = title );
			return tableList;
			
		}
		
		public static ascx_TableList show(this ascx_TableList tableList, object targetObject)	
		{			
			if (tableList.notNull() && targetObject.notNull())
			{
				tableList.clearTable();
				tableList.title("{0}".format(targetObject.typeFullName()));  
				tableList.add_Columns("name","value"); 
				foreach(var property in PublicDI.reflection.getProperties(targetObject))
					tableList.add_Row(property.Name, targetObject.prop(property.Name).str());
				tableList.makeColumnWidthMatchCellWidth();					
			}
			return tableList;
		}


		//screenshots
		
		public static string saveImageFromClipboard(this object _object)
		{
			var sync = new AutoResetEvent(false);
			string savedImage = null;
			O2Thread.staThread(
				()=>{
						var bitmap = new Control().fromClipboardGetImage();  
						if (bitmap.notNull())
						{
							savedImage = bitmap.save();
							savedImage.toClipboard(); 
							"Image in clipboard was saved to: {0}".info(savedImage);
						}
						sync.Set();							
					});
					
			sync.WaitOne(2000);
			
			return savedImage;
		}
		
		// we need to do this because the clipboard can only be accessed from an STA thread
		public static string clipboardText_Get(this object _object)
		{
			var sync = new AutoResetEvent(false);
			string clipboardText = null;
			O2Thread.staThread(
				()=>{
						clipboardText = O2Forms.getClipboardText();						
						sync.Set();							
					});					
			sync.WaitOne(2000);		
			return clipboardText;
		}
		// poing existing toClipboard(this string _) to this
		public static string clipboardText_Set(this string newClipboardText)
		{
			var sync = new AutoResetEvent(false);			
			O2Thread.staThread(
				()=>{
						O2Forms.setClipboardText(newClipboardText);
						sync.Set();							
					});					
			sync.WaitOne(2000);		
			return newClipboardText;
		}
		*/
	}	   
}
    	