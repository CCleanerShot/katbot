import { OrderType } from '$lib/enums';

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

	static ToClass(item: BazaarItem): BazaarItem {
		return new BazaarItem(item.ID, item.Name, item.OrderType, item.Price, item.RemovedAfter, item.UserId);
	}

	static ToType(item: BazaarItem): BazaarItem {
		const { ID, Name, OrderType, Price, RemovedAfter, UserId } = item;
		return { ID, Name, OrderType, Price, RemovedAfter, UserId } as BazaarItem;
	}

	public OrderTypeString(): string {
		switch (this.OrderType) {
			case OrderType.INSTA:
				return 'INSTA';
			case OrderType.ORDER:
				return 'ORDER';
		}
	}
}
