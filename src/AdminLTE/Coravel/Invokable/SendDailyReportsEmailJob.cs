using Coravel.Invocable;
using Coravel.Mailer.Mail.Interfaces;

namespace AdminLTE.Coravel.Invokable
{
    public class SendDailyReportsEmailJob : IInvocable
    {
        private IMailer _mailer;
        private IUserRepository _repo;

        // Each param injected from the service container ;)
        public SendDailyReportsEmailJob(IMailer mailer, IUserRepository repo)
        {
            this._mailer = mailer;
            this._repo = repo;
        }

        public async Task Invoke()
        {
            var users = await this._repo.GetUsersAsync();

            foreach (var user in users)
            {
                var mailable = new NightlyReportMailable(user);
                await this._mailer.SendAsync(mailable);
            }
        }
    }
}
