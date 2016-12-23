namespace Suneco.SwitchingLinkProvider.Test
{
    using FluentAssertions;
    using Sitecore;
    using Sitecore.FakeDb;
    using Xunit;

    /// <summary>
    /// FakeDb tests
    /// </summary>
    public class FakeDbTests
    {
        /// <summary>
        /// Should execute fake database test.
        /// </summary>
        [Fact]
        public void ShouldExecuteFakeDbTest()
        {
            var dbItem = new DbItem("Item name");
            dbItem.Fields.Add(FieldIDs.DisplayName, "Display name");

            using (var db = new Db { dbItem })
            {
                var item = db.GetItem(dbItem.ID);

                item.Should().NotBeNull();
                item.Name.Should().Be(dbItem.Name);
            }
        }
    }
}
