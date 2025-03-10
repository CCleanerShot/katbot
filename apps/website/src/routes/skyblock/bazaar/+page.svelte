<script lang="ts">
	import BazaarColumn from '$lib/components/BazaarColumn.svelte';
</script>

<div class="flex">
	<BazaarColumn action="GET=>/api/bazaar/buy" actionDelete="DELETE=>/api/bazaar/buy" type="BUYS" />
	<BazaarColumn action="GET=>/api/bazaar/sell" actionDelete="DELETE=>/api/bazaar/sell" type="SELLS" />
</div>
