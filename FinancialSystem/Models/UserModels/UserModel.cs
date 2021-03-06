﻿
using System.Diagnostics;
using FluentNHibernate.Mapping;
using FinancialSystem.Models.Enums;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using FinancialSystem.Models.UserModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialSystem.Models {
	[DebuggerDisplay("{FirstName} {LastName}")]
	public class UserModel : IdentityUser {
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string Email { get { return UserName; } }
		public virtual string Password { get; set; }
		public virtual long CurrentRole { get; set; }
		
		public virtual String Name() {
			return ((FirstName ?? "").Trim() + " " + (LastName ?? "").Trim()).Trim();
		}
		public UserModel() {
			
			CreateTime = DateTime.UtcNow;
		}

		public virtual DateTime CreateTime { get; set; }
		public virtual DateTime? DeleteTime { get; set; }
		public virtual string CreatedBy { get; set; }

		public virtual bool IsAdmin { get; set; }
		public virtual bool IsActive { get; set; }
		public virtual ICollection<UserRole> Roles { get; set; }
		public virtual ICollection<UserLogin> Logins { get; set; }
		public virtual EmployeeModel employee { get; set; }

		public class UserModelMap : ClassMap<UserModel> {
			public UserModelMap() {
				Id(x => x.Id).CustomType(typeof(string)).GeneratedBy.Custom(typeof(GuidStringGenerator)).Length(36);
				Map(x => x.UserName).Index("UserName_IDX").Length(400).UniqueKey("uniq");
				Map(x => x.FirstName).Not.LazyLoad();
				Map(x => x.LastName).Not.LazyLoad();
				Map(x => x.PasswordHash);
				Map(x => x.CurrentRole);
				Map(x => x.IsAdmin);
				Map(x => x.DeleteTime);
				Map(x => x.CreateTime);
				Map(x => x.SecurityStamp).Index("SecurityStamp_IDX").Length(400).UniqueKey("uniq");
				HasMany(x => x.Logins).Cascade.SaveUpdate();
				HasMany(x => x.Roles).Cascade.SaveUpdate();
				References(x => x.employee, "employee").Cascade.SaveUpdate();
			}
		}



		
	}

}
