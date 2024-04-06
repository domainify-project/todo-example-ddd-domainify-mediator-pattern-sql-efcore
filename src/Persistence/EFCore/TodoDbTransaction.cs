namespace Persistence
{
    public class TodoDbTransaction : DbTransaction
    {
        public TodoDbTransaction(TodoDbContext context) 
            : base(context) { }
    }
}