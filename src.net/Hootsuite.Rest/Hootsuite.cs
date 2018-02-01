using Hootsuite.Rest.Api;

namespace Hootsuite.Rest
{
    public class Hootsuite
    {
        Connection _connection;
        ConnectionOwly _connectionOwly;

        public Hootsuite(dynamic options)
        {
            _connection = new Connection(options);
            _connectionOwly = new ConnectionOwly(options);
            //
            me = new Me(this, _connection);
            media = new Media(this, _connection);
            members = new Members(this, _connection);
            messages = new Messages(this, _connection);
            organizations = new Organizations(this, _connection);
            scim = new Scim(this, _connection);
            socialProfiles = new SocialProfiles(this, _connection);
            teams = new Teams(this, _connection);
            owly = new Owly(this, _connectionOwly);
        }

        public Me me { get; private set; }
        public Media media { get; private set; }
        public Members members { get; private set; }
        public Messages messages { get; private set; }
        public Organizations organizations { get; private set; }
        public Scim scim { get; private set; }
        public SocialProfiles socialProfiles { get; private set; }
        public Teams teams { get; private set; }
        public Owly owly { get; private set; }
    }
}
