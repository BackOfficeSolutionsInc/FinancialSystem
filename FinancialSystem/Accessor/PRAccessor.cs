﻿using FinancialSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialSystem.Accessor {
	public class PRAccessor {
		public string GetRequisitionNo(EmployeeModel Requestor,long id) {
			return Requestor.Company.CompanyCode + DateTime.UtcNow.Month.ToString().PadLeft(2, '0') + id.ToString().PadLeft(5, '0');
		}
	}
}