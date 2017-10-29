using SQLite;

namespace App
{
   public class Items
    {
        [PrimaryKey]
        public string item_id { get; set; }
        public string item_owner { get; set; }

        public string item_kind { get; set; }

        public override string ToString()
        {
            return string.Format("[Item: item_id={0}, item_owner={1}, item_kind={2}]", item_id, item_owner, item_kind);
        }
    }
}