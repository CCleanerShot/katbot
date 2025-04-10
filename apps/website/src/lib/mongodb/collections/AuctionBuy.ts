import { AuctionItem } from '../AuctionItem';
import { AuctionTag } from '../AuctionTag';

export class AuctionBuy extends AuctionItem {
	public static Equals(c1: AuctionBuy, c2: AuctionBuy) {
		if (c1.ID != c2.ID) {
			return false;
		}

		if (c1.ID != c2.ID) {
			return false;
		}

		if (c1.ID != c2.ID) {
			return false;
		}

		if (c1.ID != c2.ID) {
			return false;
		}
		if (c1.ID != c2.ID) {
			return false;
		}

		if (c1.ID != c2.ID) {
			return false;
		}

		for (const tag of c2.AuctionTags) {
			if (!c1.AuctionTags.find((e) => AuctionTag.Equals(e, tag))) {
				return false;
			}
		}

		return true;
	}
}
