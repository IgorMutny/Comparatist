using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class CategoryCascadeDeleteHandler : CascadeDeleteHandler<Category>
    {
        public CategoryCascadeDeleteHandler(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> GetBoundedRecords(Category record)
        {
            var rootRepository = Database.GetRepository<Root>();

            var updatedRoots = rootRepository.GetAll().Where(x => x.CategoryIds.Contains(record.Id));

            foreach (var root in updatedRoots)
            {
                root.CategoryIds.Remove(record.Id);
                rootRepository.Update(root);
            }

            return Database.GetRepository<Category>()
                .GetAll()
                .Where(x => x.ParentId == record.Id)
                .ToList();
        }
    }
}
