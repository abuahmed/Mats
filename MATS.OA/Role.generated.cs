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
using MATS.OA;


namespace MATS.OA	
{
	public partial class Role
	{
		private int _roleId;
		public virtual int RoleId 
		{ 
		    get
		    {
		        return this._roleId;
		    }
		    set
		    {
		        this._roleId = value;
		    }
		}
		
		private string _roleDescription;
		public virtual string RoleDescription 
		{ 
		    get
		    {
		        return this._roleDescription;
		    }
		    set
		    {
		        this._roleDescription = value;
		    }
		}
		
		private string _roleDescriptionShort;
		public virtual string RoleDescriptionShort 
		{ 
		    get
		    {
		        return this._roleDescriptionShort;
		    }
		    set
		    {
		        this._roleDescriptionShort = value;
		    }
		}
		
		private int _enabled;
		public virtual int Enabled 
		{ 
		    get
		    {
		        return this._enabled;
		    }
		    set
		    {
		        this._enabled = value;
		    }
		}
		
		private DateTime? _dateRecordCreated;
		public virtual DateTime? DateRecordCreated 
		{ 
		    get
		    {
		        return this._dateRecordCreated;
		    }
		    set
		    {
		        this._dateRecordCreated = value;
		    }
		}
		
		private DateTime? _dateLastModified;
		public virtual DateTime? DateLastModified 
		{ 
		    get
		    {
		        return this._dateLastModified;
		    }
		    set
		    {
		        this._dateLastModified = value;
		    }
		}
		
		private IList<User> _users = new List<User>();
		public virtual IList<User> Users 
		{ 
		    get
		    {
		        return this._users;
		    }
		}
		
	}
}
