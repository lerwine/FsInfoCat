using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DevHelperGUI
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> GetDataBoundItemsOfType<T>(this IEnumerable<DataGridViewRow> rows) => rows.Select(r => r.DataBoundItem).OfType<T>();

        public static IEnumerable<T> GetDataBoundItemsOfType<T>(this DataGridViewSelectedRowCollection selectedRows) =>
            GetDataBoundItemsOfType<T>(selectedRows.Cast<DataGridViewRow>());

        public static IEnumerable<T> GetDataBoundItemsOfType<T>(this DataGridViewRowCollection rows) =>
            GetDataBoundItemsOfType<T>(rows.Cast<DataGridViewRow>());

        public static IEnumerable<Tuple<T, DataGridViewRow>> GetRowsWithDataBoundItemsOfType<T>(this IEnumerable<DataGridViewRow> rows) =>
            rows.Select(r => new { Item = r.DataBoundItem, Row = r }).Where(a => a.Item is T).Select(a => new Tuple<T, DataGridViewRow>((T)a.Item, a.Row));

        public static IEnumerable<Tuple<T, DataGridViewRow>> GetRowsWithDataBoundItemsOfType<T>(this DataGridViewSelectedRowCollection selectedRows) =>
            GetRowsWithDataBoundItemsOfType<T>(selectedRows.Cast<DataGridViewRow>());

        public static IEnumerable<Tuple<T, DataGridViewRow>> GetRowsWithDataBoundItemsOfType<T>(this DataGridViewRowCollection rows) =>
            GetRowsWithDataBoundItemsOfType<T>(rows.Cast<DataGridViewRow>());

        public static bool TryGetFirstDataBoundItemOfType<T>(this IEnumerable<DataGridViewRow> rows, out T item, out DataGridViewRow row)
        {
            var rowAndItem = rows.Select(r => new { Row = r, Item = r.DataBoundItem }).FirstOrDefault(a => a.Item is T);
            if (rowAndItem is null)
            {
                row = null;
                item = default;
                return false;
            }
            item = (T)rowAndItem.Item;
            row = rowAndItem.Row;
            return true;
        }

        public static bool TryGetFirstDataBoundItemOfType<T>(this DataGridViewSelectedRowCollection selectedRows, out T item, out DataGridViewRow row) =>
            TryGetFirstDataBoundItemOfType(selectedRows.Cast<DataGridViewRow>(), out item, out row);

        public static bool TryGetFirstDataBoundItemOfType<T>(this DataGridViewRowCollection rows, out T item, out DataGridViewRow row) =>
            TryGetFirstDataBoundItemOfType(rows.Cast<DataGridViewRow>(), out item, out row);

        public static bool TryGetFirstDataBoundItemOfType<T>(this IEnumerable<DataGridViewRow> rows, out T item)
        {
            using (IEnumerator<T> enumerator = rows.GetDataBoundItemsOfType<T>().GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    item = enumerator.Current;
                    return true;
                }
            }
            item = default;
            return false;
        }

        public static bool TryGetFirstDataBoundItemOfType<T>(this DataGridViewSelectedRowCollection selectedRows, out T item) =>
            TryGetFirstDataBoundItemOfType(selectedRows.Cast<DataGridViewRow>(), out item);

        public static bool TryGetFirstDataBoundItemOfType<T>(this DataGridViewRowCollection rows, out T item) =>
            TryGetFirstDataBoundItemOfType(rows.Cast<DataGridViewRow>(), out item);

        public static bool TryGetSelectedOrFirstDataBoundItemOfType<T>(this DataGridView dataGridView, out T item, out DataGridViewRow row) =>
            TryGetFirstDataBoundItemOfType(dataGridView.SelectedRows, out item, out row) || TryGetFirstDataBoundItemOfType(dataGridView.Rows, out item, out row);

    }
}
