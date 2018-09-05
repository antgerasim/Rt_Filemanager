using DevExpress.Web.Mvc;
using DevExtremeFixed.Data;
using DevExtremeFixed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevExtremeFixed.Controllers
{
    public class GridController : Controller
    {
        RTCRM Db = new RTCRM();

        public ActionResult Index(string parenttableid, string contractstatus)
        {
            //http://localhost:50895/?parenttableid=3D317583-36B0-E811-80D3-DBE78F6B8753
            //ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!";
            var ptid = parenttableid == null ? "3D317583-36B0-E811-80D3-DBE78F6B8753" : parenttableid;
            var cstatus = contractstatus == null ? "2" : contractstatus;
            Session["parenttableid"] = ptid;
            //Session["contractstatus"] = cstatus;
            return View(new IndexViewModel(ptid));
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingPartial(string parenttableid)
        {
            //3D317583 - 36B0 - E811 - 80D3 - DBE78F6B8753 //my choice 359C8C83-74AC-E811-80D3-DBE78F6B8753
            if (Request.IsAjaxRequest())
            {
                var ptid = Session["parenttableid"] as string;
                //var cstatus = Session["contractstatus"] as string;
                var ajaxmodel = GetEditableViewModelContracts(new Guid(ptid));

                return PartialView("_BatchEditingPartial", ajaxmodel);
            }
            var parentTable = Db.new_contract_planBase.FirstOrDefault(x => x.new_contract_planId == new Guid(parenttableid));
            var contractStatus = parentTable.statuscode;

            Session["contractstatus"] = parentTable.statuscode ?? 2;
            var model = GetEditableViewModelContracts(new Guid(parenttableid));
            var urlTest = Request.RawUrl;
            return PartialView("_BatchEditingPartial", model);
        }

        // Apply all changes made on the client side to a data source.
        [HttpPost, ValidateInput(false)]
        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<EditableContract, Guid> updateValues)
        {
            var dataContextModelContracts = GetDataContextModelContracts();
            // Insert all added values. 
            foreach (var contract in updateValues.Insert)
            {
                if (updateValues.IsValid(contract))
                {
                    try
                    {
                        InsertContract(contract, dataContextModelContracts);
                        Db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        updateValues.SetErrorText(contract, e.Message);
                    }
                }
            }
            // Update all edited values. 
            foreach (var contract in updateValues.Update)
            {
                if (updateValues.IsValid(contract))
                {
                    try
                    {
                        UpdateContract(contract);
                        Db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        updateValues.SetErrorText(contract, e.Message);
                    }
                }
            }

            // Delete all values that were deleted on the client side from the data source. 
            foreach (var ContrGUID in updateValues.DeleteKeys)
            {
                try
                {
                    DeleteContract(ContrGUID, dataContextModelContracts);
                    Db.SaveChanges();
                }
                catch (Exception e)
                {
                    updateValues.SetErrorText(ContrGUID, e.Message);
                }
            }
            var parenttableid = Session["parenttableid"] as string;
            //var contractstatus = Session["contractstatus"] as string;
            return BatchEditingPartial(parenttableid);
        }

        private void DeleteContract(Guid contrGuid, IList<new_contract_plan_productBase> dataContextModelContracts)
        {
            var dataContract = GetDataContextModelContract(contrGuid);
            if (dataContract != null)
            {
                GetDataContextModelContracts().Remove(dataContract);
            }
        }

        private void InsertContract(EditableContract editContract, IList<new_contract_plan_productBase> dataContextModelContracts)
        {
            var dataContextModelContract = new new_contract_plan_productBase();
            //initialize childs for null reference errors
            dataContextModelContract.new_d_product_groupsBase = new new_d_product_groupsBase();
            dataContextModelContract.new_d_product_catalogBase = new new_d_product_catalogBase();

            dataContextModelContract.new_contract_plan_productId = Guid.NewGuid(); //todo check if EF or db add guid automatically
            dataContextModelContract.new_d_product_groupsBase.new_name = editContract.ProductGroupProduct;
            dataContextModelContract.new_d_product_catalogBase.new_name = editContract.Product;
            dataContextModelContract.new_service_1_quarter = editContract.Service1Quarter;
            dataContextModelContract.new_consulting_1_quarter = editContract.Consult1Quarter;
            //dataContextModelContracts.Add(dataContextModelContract);
            Db.new_contract_plan_productBase.ToList().Add(dataContextModelContract);
        }

        private void UpdateContract(EditableContract editContract)
        {
            var dataContract = Db.new_contract_plan_productBase.FirstOrDefault(it => it.new_contract_plan_productId == editContract.ContrGuid);

            if (dataContract != null)
            {
                /*mapping from viewmodel to dataModel*/
                dataContract.new_d_product_groupsBase.new_name = editContract.ProductGroupProduct ?? ""; //null check 
                dataContract.new_d_product_catalogBase.new_name = editContract.Product ?? ""; // null check
                dataContract.new_service_1_quarter = editContract.Service1Quarter;
                dataContract.new_service_2_quarter = editContract.Service2Quarter;
                dataContract.new_service_3_quarter = editContract.Service3Quarter;
                dataContract.new_service_4_quarter = editContract.Service4Quarter;
               dataContract.new_consulting_1_quarter = editContract.Consult1Quarter;
                dataContract.new_consulting_2_quarter = editContract.Consult2Quarter;
                dataContract.new_consulting_3_quarter = editContract.Consult3Quarter;
                dataContract.new_consulting_4_quarter = editContract.Consult4Quarter;
                dataContract.new_service_year = editContract.NewServiceYear;
                dataContract.new_year_sum = editContract.NewConsultYear;
                dataContract.new_product_sum_consulting = editContract.NewProductTotalConsult;
                dataContract.new_product_sum_service = editContract.NewProductTotalService;
                //dataContract.sum
            }
        }

        private IList<EditableContract> GetEditableViewModelContracts(Guid filterId)
        {
            IQueryable<EditableContract> query = Db.new_contract_plan_productBase
              .Where(x => x.new_link_contract_plan_year_id == filterId)
              .Select(dataContextModelContract => new EditableContract
              {
                  ContrGuid = dataContextModelContract.new_contract_plan_productId,
                  ProductGroupProduct = dataContextModelContract.new_d_product_groupsBase.new_name,
                  Product = dataContextModelContract.new_d_product_catalogBase.new_name,
                  Service1Quarter = dataContextModelContract.new_service_1_quarter,
                  Service2Quarter = dataContextModelContract.new_service_2_quarter,
                  Service3Quarter = dataContextModelContract.new_service_3_quarter,
                  Service4Quarter = dataContextModelContract.new_service_4_quarter,
                  Consult1Quarter = dataContextModelContract.new_consulting_1_quarter,
                  Consult2Quarter = dataContextModelContract.new_consulting_2_quarter,
                  Consult3Quarter = dataContextModelContract.new_consulting_3_quarter,
                  Consult4Quarter = dataContextModelContract.new_consulting_4_quarter,
                  NewServiceYear = dataContextModelContract.new_service_year,
                  NewConsultYear = dataContextModelContract.new_year_sum,
                  NewProductTotalConsult = dataContextModelContract.new_product_sum_consulting,
                  NewProductTotalService = dataContextModelContract.new_product_sum_service,
                  NewYearTotal = dataContextModelContract.new_year_sum,
                  FirstQuartalTotal = dataContextModelContract.new_service_1_quarter + dataContextModelContract.new_consulting_1_quarter,
                  SecondQuartalTotal = dataContextModelContract.new_service_2_quarter + dataContextModelContract.new_consulting_2_quarter,
                  ThirdQuartalTotal = dataContextModelContract.new_service_3_quarter + dataContextModelContract.new_consulting_3_quarter,
                  FourthQuartalTotal = dataContextModelContract.new_service_4_quarter + dataContextModelContract.new_consulting_4_quarter,
                  StatusCode = dataContextModelContract.statuscode
              });
            var contracts = query.ToList();
            return contracts;
        }

        private IList<new_contract_plan_productBase> GetDataContextModelContracts()
        {
            return Db.new_contract_plan_productBase.ToList(); ;
        }

        private new_contract_plan_productBase GetDataContextModelContract(Guid contrGuid)
        {
            return Db.new_contract_plan_productBase.FirstOrDefault(contract => contract.new_contract_plan_productId == contrGuid);
        }
    }
}