// ReSharper disable All
using MixERP.Net.Framework;
using MixERP.Net.Entities.Office;
using MixERP.Net.Schemas.Office.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MixERP.Net.ApplicationState.Cache;
using MixERP.Net.Common.Extensions;
using PetaPoco;
using MixERP.Net.EntityParser;
namespace MixERP.Net.Api.Office
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function GetCostCenterIdByCostCenterCode.
    /// </summary>
    [RoutePrefix("api/v1.5/office/procedures/get-cost-center-id-by-cost-center-code")]
    public class GetCostCenterIdByCostCenterCodeController : ApiController
    {
        /// <summary>
        /// Login id of application user accessing this API.
        /// </summary>
        public long _LoginId { get; set; }

        /// <summary>
        /// User id of application user accessing this API.
        /// </summary>
        public int _UserId { get; set; }

        /// <summary>
        /// Currently logged in office id of application user accessing this API.
        /// </summary>
        public int _OfficeId { get; set; }

        /// <summary>
        /// The name of the database where queries are being executed on.
        /// </summary>
        public string _Catalog { get; set; }

        /// <summary>
        ///     The GetCostCenterIdByCostCenterCode repository.
        /// </summary>
        private readonly IGetCostCenterIdByCostCenterCodeRepository repository;

        public class Annotation
        {
            public string CostCenterCode { get; set; }
        }


        public GetCostCenterIdByCostCenterCodeController()
        {
            this._LoginId = AppUsers.GetCurrent().View.LoginId.ToLong();
            this._UserId = AppUsers.GetCurrent().View.UserId.ToInt();
            this._OfficeId = AppUsers.GetCurrent().View.OfficeId.ToInt();
            this._Catalog = AppUsers.GetCurrentUserDB();

            this.repository = new GetCostCenterIdByCostCenterCodeProcedure
            {
                _Catalog = this._Catalog,
                _LoginId = this._LoginId,
                _UserId = this._UserId
            };
        }

        public GetCostCenterIdByCostCenterCodeController(IGetCostCenterIdByCostCenterCodeRepository repository, string catalog, LoginView view)
        {
            this._LoginId = view.LoginId.ToLong();
            this._UserId = view.UserId.ToInt();
            this._OfficeId = view.OfficeId.ToInt();
            this._Catalog = catalog;

            this.repository = repository;
        }

        /// <summary>
        ///     Creates meta information of "get cost center id by cost center code" annotation.
        /// </summary>
        /// <returns>Returns the "get cost center id by cost center code" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/office/procedures/get-cost-center-id-by-cost-center-code/annotation")]
        public EntityView GetAnnotation()
        {
            if (this._LoginId == 0)
            {
                return new EntityView();
            }
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                                {
                                        new EntityColumn { ColumnName = "_cost_center_code",  PropertyName = "CostCenterCode",  DataType = "string",  DbDataType = "text",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 }
                                }
            };
        }




        [AcceptVerbs("POST")]
        [Route("execute")]
        [Route("~/api/office/procedures/get-cost-center-id-by-cost-center-code/execute")]
        public int Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.CostCenterCode = annotation.CostCenterCode;


                return this.repository.Execute();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (MixERPException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    }
}