import { TagType } from '$lib/enums';

export class AuctionTag {
	/** The name of the tag. */
	Name: string;
	/** The type of tag. */
	Type: TagType;
	/** The value for this tag. */
	Value: string;

	constructor(Name: string, Type: TagType, Value: string) {
		this.Name = Name;
		this.Type = Type;
		this.Value = Value;
	}

	public static Equals(c1: AuctionTag, c2: AuctionTag): boolean {
		if (c1.Name != c2.Name) {
			return false;
		}

		if (c1.Type != c2.Type) {
			return false;
		}

		if (c1.Value != c2.Value) {
			return false;
		}

		return true;
	}
}
