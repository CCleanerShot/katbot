import { Database } from "./supabase/types";

export namespace DB {
	export type InsertItem = Database["public"]["Tables"]["auction_items"]["Insert"];
	export type RowItem = Database["public"]["Tables"]["auction_items"]["Row"];
	export type UpdatePrice = Database["public"]["Tables"]["auction_prices"]["Update"];
	export type InsertPrice = Database["public"]["Tables"]["auction_prices"]["Insert"];
	export type RowPrice = Database["public"]["Tables"]["auction_prices"]["Row"];
}
