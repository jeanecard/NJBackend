using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Commun;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NJBudgetWBackend.Services
{
    public class AppartenanceService : IAppartenanceService
    {
        private static Appartenance _commun = new Appartenance() { Id = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID), Caption = Constant.APPARTENANCE_COMMUN_CAPTION };
        private static Appartenance _jean = new Appartenance() { Id = Guid.Parse(Constant.APPARTENANCE_JEAN_GUID), Caption = Constant.APPARTENANCE_JEAN_CAPTION };
        private static Appartenance _nad = new Appartenance() { Id = Guid.Parse(Constant.APPARTENANCE_NADEGE_GUID), Caption = Constant.APPARTENANCE_NADEGE_CAPTION };
        private static Appartenance _thomas = new Appartenance() { Id = Guid.Parse(Constant.APPARTENANCE_THOMAS_GUID), Caption = Constant.APPARTENANCE_THOMAS_CAPTION };

        public Appartenance GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            switch (id.ToString())
            {
                case Constant.APPARTENANCE_COMMUN_GUID:
                    return _commun;
                case Constant.APPARTENANCE_JEAN_GUID:
                    return _jean;
                case Constant.APPARTENANCE_NADEGE_GUID:
                    return _nad;
                case Constant.APPARTENANCE_THOMAS_GUID:
                    return _thomas;
                default:
                    return null;
            }
        }

        public IEnumerable<Appartenance> Get()
        {
            return new List<Appartenance> {
                _commun,
                _jean,
                _nad,
                _thomas,
            };

        }
    }
}
