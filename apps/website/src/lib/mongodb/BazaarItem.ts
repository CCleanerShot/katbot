import { OrderType } from '$lib/enums';
import { cacheState } from '$lib/states/cacheState.svelte';

export class BazaarItem {
	/** Hypixel ID of the item. */
	ID: string;
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

	constructor(_ID: string, _Name: string, _OrderType: OrderType, _Price: bigint, _RemovedAfter: boolean, _UserId: bigint) {
		this.ID = _ID;
		this.Name = _Name;
		this.OrderType = _OrderType;
		this.Price = _Price;
		this.RemovedAfter = _RemovedAfter;
		this.UserId = _UserId;
	}

	static Empty(): BazaarItem {
		return { ID: '', Name: '', OrderType: OrderType.ORDER, Price: 0n, RemovedAfter: true, UserId: 0n };
	}

	static ToClass(item: BazaarItem): BazaarItem {
		return new BazaarItem(item.ID, item.Name, item.OrderType, item.Price, item.RemovedAfter, item.UserId);
	}

	static ToType(item: BazaarItem): BazaarItem {
		const { Name, OrderType, Price, RemovedAfter, UserId } = item;
		const ID = cacheState.BAZAAR.find((e) => e.Name === item.Name)?.ID!;
		return { ID, Name, OrderType, Price, RemovedAfter, UserId } as BazaarItem;
	}

	public static OrderTypeString(item: BazaarItem): string {
		switch (item.OrderType) {
			case OrderType.INSTA:
				return 'INSTA';
			case OrderType.ORDER:
				return 'ORDER';
		}
	}
}
