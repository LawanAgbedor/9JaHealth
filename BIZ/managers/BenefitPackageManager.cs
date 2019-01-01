using DAL.com._9jahealth.data.dao;
using DAL.com._9jahealth.data.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ.managers
{
    public class BenefitPackageManager
    {
        #region BenefitPackageCategory

        public static BenefitPackageCategory GetByBenefitPackageCategory(int packageCategoryId)
        {
            var record = BenefitPackageCategoryDao.GetByID(packageCategoryId);
            return record;
        }

        public static IList<BenefitPackageCategory> GetAllBenefitPackageCategory()
        {
            var recordList = BenefitPackageCategoryDao.GetAll();
            return recordList;
        }

        #endregion BenefitPackageCategory


        #region BenefitPackageHCSA

        public static BenefitPackageHCSA GetBenefitPackageHCSA(int packageHCSAId)
        {
            var record = BenefitPackageHCSADao.GetByID(packageHCSAId);
            return record;
        }

        #endregion BenefitPackageHCSA


        #region BenefitPackageInterventionProgram

        public static BenefitPackageInterventionProgram GetBenefitPackageInterventionProgram(int packageInterventionProgramId)
        {
            var record = BenefitPackageInterventionProgramDao.GetByID(packageInterventionProgramId);
            return record;
        }

        #endregion BenefitPackageInterventionProgram


        #region BenefitPackageProgramService

        public static BenefitPackageProgramService GetBenefitPackageProgramService(int packageProgramServiceId)
        {
            var record = BenefitPackageProgramServiceDao.GetByID(packageProgramServiceId);
            return record;
        }

        #endregion BenefitPackageProgramService
    }
}
