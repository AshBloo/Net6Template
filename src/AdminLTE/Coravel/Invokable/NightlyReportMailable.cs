using Coravel.Mailer.Mail;
using static AdminLTE.Coravel.Invokable.SendNightlyReportsEmailJob;

namespace AdminLTE.Coravel.Invokable
{
    public class NightlyReportMailable : Mailable<UserModel>
    {
        private UserModel _user;

        public NightlyReportMailable(UserModel user) => this._user = user;

        public override void Build()
        {
            this.To(this._user)
                .From("from@test.com")
                .View("~/Views/Mail/NewUser.cshtml", this._user);
        }
    }
}