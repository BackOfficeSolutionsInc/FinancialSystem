﻿
using System.Diagnostics;
using FluentNHibernate.Mapping;
using FinancialSystem.Models.Enums;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using FinancialSystem.Models.UserModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialSystem.NHibernate;

namespace FinancialSystem.Models {
	public class ItemModel  {
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Code { get; set; }
		public virtual string SKU { get; set; }
		public virtual string Description { get; set; }
		public virtual int Quantity { get; set; }
		public virtual string image { get; set; }
		public virtual bool InStock { get; set; }
		public virtual SupplierModel Supplier { get; set; }
		public virtual CategoryModel Category { get; set; }
		public virtual BrandModel Brand { get; set; }

		public virtual UserModel CreatedBy { get; set; }
		public ItemModel() {
			
			CreateTime = DateTime.UtcNow;
			
		}

		public virtual DateTime CreateTime { get; set; }
		public virtual DateTime? DeleteTime { get; set; }

		public class ItemModelMap : ClassMap<ItemModel> {
			public ItemModelMap() {
				Id(x => x.Id);
				Map(x => x.Name);
				Map(x => x.Code);
				Map(x => x.SKU);
				Map(x => x.Description);
				Map(x => x.Quantity);
				Map(x => x.image);
				Map(x => x.InStock);
				Map(x => x.CreateTime);
				Map(x => x.DeleteTime);
				References(p => p.CreatedBy, "CreatedBy").Cascade.SaveUpdate();
				References(p => p.Supplier, "Supplier").Cascade.SaveUpdate();
				References(p => p.Category, "Category").Cascade.SaveUpdate();
				References(p => p.Brand, "Brand").Cascade.SaveUpdate();
			}
		}



		
	}

}