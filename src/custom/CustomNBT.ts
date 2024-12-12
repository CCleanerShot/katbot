import nbt from "prismarine-nbt";
import { myUtils } from "../utils";

type NBTValue = {
	id: { value: any };
	Count: { value: number };
	tag: { value: { display: { value: { Lore: { value: { value: string[] } }; Name: { value: any } } } } };
	Damage: { value: any };
};

export class CustomNBT {
	Count: number;
	Lore: string;
	Name: string;
	Tier: string;

	/** @hideconstructor dont use constructor! need async for NBT parsing*/
	constructor(Count: number, Lore: string[], Name: string) {
		this.Count = Count;
		this.Name = myUtils.RemoveSpecialText(Name.trim());

		// before quick hotfixes
		this.Lore = myUtils.RemoveSpecialText(Lore.join("").trim());
		this.Lore = myUtils.FilterLore(this.Lore);

		const lastLine = myUtils.RemoveSpecialText(Lore[Lore.length - 1]).trim();
		/// cut off from the first space
		const cutIndex = lastLine.indexOf(" ");
		this.Tier = cutIndex === -1 ? lastLine : lastLine.slice(0, cutIndex + 1);
	}

	/** creates the NBT class item, includes auto-formatting */
	static async Create(bufferString: string): Promise<CustomNBT> {
		const data = Buffer.from(bufferString, "base64");
		const { Count, Damage, id, tag } = ((await nbt.parse(data)).parsed.value as any)!.i!.value!.value![0] as NBTValue;
		const { Lore, Name } = tag.value.display.value;

		const newNBT = new CustomNBT(Count.value, Lore.value.value, Name.value);
		return newNBT;
	}
}
