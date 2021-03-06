﻿                                          using FinancialSystem.Accessor;
using FinancialSystem.Models;
using FinancialSystem.Models.Enums;
using FinancialSystem.Utilities;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FinancialSystem.NHibernate {


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
	public class NHibernatePRStore {
		public async Task<IList<PRHeaderModel>> SearchPRAsync(string search) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					//var items = db.QueryOver<PRHeaderModel>().Where((Restrictions.On<PRHeaderModel>(x => x.RequisitionNo).IsLike(search + "%")
					//	//|| Restrictions.On<PRHeaderModel>(x => x.Requestor.LastName).IsLike(search + "%")
					//	/*|| Restrictions.On<PRHeaderModel>(x => x.Requestor.Department.Name).IsLike(search + "%")*/)
					//	&& Restrictions.On<PRHeaderModel>(x => x.DeleteTime).IsNull).JoinQueryOver<EmployeeModel>(c=>c.Requestor).Where(Restrictions.On<EmployeeModel>(k=>k.Name()).IsLike(search + "%"));
					PRHeaderModel pr = null;
					EmployeeModel emp = null;
					DepartmentModel dep = null;
					var items = db.QueryOver<PRHeaderModel>(() => pr)
						.JoinAlias(() => pr.Requestor, () => emp)
						.JoinAlias(() => emp.Department, () => dep)
						.Where(x => pr.DeleteTime==null && pr.Status==StatusType.Approved && (pr.RequisitionNo.IsLike("%" + search + "%")
						|| emp.LastName.IsLike(search + "%")
						|| dep.Name.IsLike(search+"%"))).OrderBy(()=>pr.CreateTime).Desc.Take(10);
					
					return items.List();
				}
			}
		}

		public async Task CreatePRLinesAsync(PRLinesModel line) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					db.Save(line);
					tx.Commit();
					db.Flush();
				}
			}
		}

		public async Task<IList<PRLinesModel>> PRLinesCreatedAsync(UserModel user) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					var lines = db.QueryOver<PRLinesModel>().Where(x=>x.CreatedBy==user && x.DeleteTime==null && x.Header==null);
					return lines.List();
				}
			}
		}

		public async Task DeletePRLineAsync(long Id) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					var line = db.Get<PRLinesModel>(Id);
					line.DeleteTime = DateTime.UtcNow;
					db.SaveOrUpdate(line);
					tx.Commit();
					db.Flush();
				}
			}
		}
		public async Task<PRLinesModel> GetPRLineAsync(long Id) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					return db.Get<PRLinesModel>(Id);
				}
			}
		}
		public async Task CreatePRHeaderAsync(PRHeaderModel header) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
				
					db.Save(header);
					
					var pra = new PRAccessor();
					header.RequisitionNo = pra.GetRequisitionNo(header.Requestor, header.Id);
					db.Update(header);
					tx.Commit();
					db.Flush();
				}
			}
		}
		public async Task<IList<PRAprovalModel>> FindPRAprovalAsync(PositionModel approver) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					return db.QueryOver<PRAprovalModel>().Where(x => x.Approver==approver && x.Status==StatusType.Request )
						.JoinQueryOver(x=>x.PRHeader).Where(a=>a.Status==StatusType.Request).List();
					
				}
			}
		}
		public async Task<PRAprovalModel> FindPRAprovalAsync(long Id) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					return db.Get<PRAprovalModel>(Id);

				}
			}
		}
		public async Task<IList<PRAprovalModel>> FindPRAprovalAsync(PRHeaderModel pr) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					return db.QueryOver<PRAprovalModel>().Where(x => x.PRHeader == pr ).List();

				}
			}
		}
		public async Task SaveOrUpdatePRApprovalAsync(PRAprovalModel aproval) {
			using (var db = HibernateSession.GetCurrentSession()) {
				using (var tx = db.BeginTransaction()) {
					
					db.SaveOrUpdate(aproval);
					
					tx.Commit();
					db.Flush();
				}
			}
		}
		


	}
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously