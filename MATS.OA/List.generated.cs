#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;


namespace MATS.OA	
{
	public partial class List
	{
		private int _id;
		public virtual int Id 
		{ 
		    get
		    {
		        return this._id;
		    }
		    set
		    {
		        this._id = value;
		    }
		}
		
		private int _type;
		public virtual int Type 
		{ 
		    get
		    {
		        return this._type;
		    }
		    set
		    {
		        this._type = value;
		    }
		}
		
		private string _displayName;
		public virtual string DisplayName 
		{ 
		    get
		    {
		        return this._displayName;
		    }
		    set
		    {
		        this._displayName = value;
		    }
		}
		
	}
}