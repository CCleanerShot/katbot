import type { OrderType } from "$lib/enums";

export type BazaarItem =
{
    /** Hypixel ID of the item. */
    ID: string
    /** Name of the item. */
    Name: string;
    /** The type of order the watch is for. */
    OrderType: OrderType;
    /** The price to start evaluating the threshold for alerts. */
    Price: bigint;
    /** Whether or not to remove the tracked item after successfully found. */
    RemovedAfter: boolean;
    /** The Discord ID of the user that submitted the tracking item. */
    UserId: bigint;
}