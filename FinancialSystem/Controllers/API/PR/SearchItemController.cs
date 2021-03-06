﻿using FinancialSystem.Models;
using FinancialSystem.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FinancialSystem.Controllers.API.PR {
	public class SearchItemController : ApiController {
		// GET api/<controller>
		public IEnumerable<string> Get() {
			return new string[] { "value1", "value2" };
		}

		// GET api/<controller>/5
		public string Get(int id) {
			return "value";
		}

		// POST api/<controller>
		public async Task<IList<ItemModel>> Post(SearchItemViewModel value) {
			NHibernateItemStore his = new NHibernateItemStore();			
			var search =await his.SearchItemAsync( value.searchItem);
			return search;
		}

		// PUT api/<controller>/5
		public void Put(int id,string value) {
			
		}

		// DELETE api/<controller>/5
		public void Delete(int id) {
		}
	}
}