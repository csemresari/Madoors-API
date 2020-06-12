using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Madoors.Models;

namespace Madoors.Controllers
{
    public class AccountController : BaseAPIController
    {
        [HttpGet]
        public AccountModel Login(string entranceID, string username, string password, string deviceKey)
        {
            AccountResultModel result = new AccountResultModel();
            result.Status = AccountResultStatus.UNKNOWN_ERROR;

            try
            {
                username = username.Trim();
                entranceID = entranceID.Trim();
                deviceKey = deviceKey.Trim();

                PersistencyManager db = m_Persistency_Manager;

                if (db != null)
                {
                    result.Account = GetAccount(db, entranceID, username, password, deviceKey, 1);
                    if (result.Account != null)
                    {
                        result.Status = AccountResultStatus.VALID_LOGIN;
                    }

                    else
                    {
                        result.Status = AccountResultStatus.INVALID_USERNAME_PASSWORD;
                    }
                }
                else
                {
                    result.Status = AccountResultStatus.INVALID_SERVER;
                }
            }
            catch (Exception exc)
            {

                result.Status = AccountResultStatus.UNKNOWN_ERROR;
            }

            return result.Account;
        }

        public AccountModel GetAccount(PersistencyManager db, String entranceID, String username, String password, String deviceKey, int bos)
        {
            AccountModel testAccount = new AccountModel();

            testAccount = (from lg in db.LOGIN
                           where lg.deviceKey == deviceKey
                           select new AccountModel
                           {
                               entranceID = lg.entranceID,
                               username = lg.username,
                               password = lg.password,
                               entranceName = lg.ENTRANCE.entranceName,
                               deviceKey = lg.deviceKey,
                               branchName = lg.ENTRANCE.BRANCH.branchName,
                               branchGuid = lg.ENTRANCE.branchID
                           }).FirstOrDefault();

            if (testAccount.username == username && testAccount.password == password && testAccount.deviceKey == deviceKey)
            {

                TOKEN aToken = new TOKEN();
                aToken.entranceID = testAccount.entranceID;
                aToken.tokenGenerationTime = DateTime.Now;
                aToken.tokenID = Guid.NewGuid();

                db.TOKEN.AddObject(aToken);
                db.SaveChanges();

                testAccount.tokenID = aToken.tokenID.ToString();

                testAccount.branchID = testAccount.branchGuid.ToString();
                testAccount.username = "";
                testAccount.password = "";
                testAccount.deviceKey = "";
                testAccount.branchGuid = null;
                return testAccount;
            }
            else
            {
                return null;
            }
        }
    }
}
