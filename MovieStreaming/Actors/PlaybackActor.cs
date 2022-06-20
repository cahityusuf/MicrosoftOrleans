using MovieStreaming;
using MovieStreaming.Messages;
using Proto;

namespace MovieStreaming.Actors
{
    public class PlaybackActor : IActor
    {
        public PlaybackActor() => Console.WriteLine("Creating a PlaybackActor");
        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started msg:
                    ProcessStartedMessage(msg);
                    break;

                case PlayMovieMessage msg:
                    ProcessPlayMovieMessage(msg);
                    break;

                case Recoverable msg:
                    ProcessRecoverableMessage(context, msg);
                    break;
            }
            return Task.CompletedTask;
        }

        private void ProcessStartedMessage(Started msg)
        {
            ColorConsole.WriteLineGreen("PlaybackActor Started");
        }

        private void ProcessPlayMovieMessage(PlayMovieMessage msg)
        {
            ColorConsole.WriteLineYellow($"PlayMovieMessage {msg.MovieTitle} for user {msg.UserId}");
        }

        private void ProcessRecoverableMessage(IContext context, Recoverable msg)
        {
            PID child;

            if (context.Children == null || context.Children.Count == 0)
            {
                var props = Props.FromProducer(() => new ChildActor());
                child = context.Spawn(props);
            }
            else
            {
                child = context.Children.First();
            }

            context.Forward(child);
        }
    }
}