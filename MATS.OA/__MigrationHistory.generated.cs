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
	public partial class __MigrationHistory
	{
		private string _migrationId;
		public virtual string MigrationId 
		{ 
		    get
		    {
		        return this._migrationId;
		    }
		    set
		    {
		        this._migrationId = value;
		    }
		}
		
		private string _contextKey;
		public virtual string ContextKey 
		{ 
		    get
		    {
		        return this._contextKey;
		    }
		    set
		    {
		        this._contextKey = value;
		    }
		}
		
		private byte[] _model;
		public virtual byte[] Model 
		{ 
		    get
		    {
		        return this._model;
		    }
		    set
		    {
		        this._model = value;
		    }
		}
		
		private string _productVersion;
		public virtual string ProductVersion 
		{ 
		    get
		    {
		        return this._productVersion;
		    }
		    set
		    {
		        this._productVersion = value;
		    }
		}
		
	}
}
