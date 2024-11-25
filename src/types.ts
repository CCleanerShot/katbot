import { Database } from "./supabase/types";

export namespace DB {
	export type InsertItem = Database["public"]["Tables"]["auction_items"]["Insert"];
	export type RowItem = Database["public"]["Tables"]["auction_items"]["Row"];
	export type UpdateItem = Database["public"]["Tables"]["auction_items"]["Update"];
}
