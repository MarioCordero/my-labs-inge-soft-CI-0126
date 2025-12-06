<template>
  <div
    :class="[
      'relative rounded-lg border transition-all duration-200',
      isSoldOut
        ? 'border-border/30 bg-card/40 opacity-50'
        : 'border-border bg-card hover:border-accent/50 hover:shadow-lg hover:shadow-accent/10'
    ]"
  >
    <!-- Sold Out Badge -->
    <div v-if="isSoldOut" class="absolute inset-0 flex items-center justify-center rounded-lg bg-black/60 backdrop-blur-sm">
      <div class="text-center">
        <p class="text-xl font-bold text-destructive">SOLD OUT</p>
      </div>
    </div>

    <div class="p-6">
      <!-- Coffee Icon -->
      <div class="mb-4 inline-flex h-16 w-16 items-center justify-center rounded-lg bg-accent/10">
        <svg class="h-8 w-8 text-accent" fill="currentColor" viewBox="0 0 24 24">
          <path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 16H5V5h14v14z" />
        </svg>
      </div>

      <!-- Name and Price -->
      <h3 class="text-xl font-semibold mb-1">{{ coffee.name }}</h3>
      <p class="text-2xl font-bold text-accent mb-4">₡{{ coffee.price.toLocaleString() }}</p>

      <!-- Stock -->
      <p :class="['text-sm mb-6', isSoldOut ? 'text-destructive' : 'text-muted-foreground']">
        {{ isSoldOut ? 'No stock' : `${coffee.stock} available` }}
      </p>

      <!-- Quantity Selector -->
      <div v-if="!isSoldOut" class="flex items-center gap-3 mb-4">
        <button
          @click="() => onUpdateQuantity(coffee.id, quantity - 1)"
          :disabled="quantity === 0"
          class="flex h-10 w-10 items-center justify-center rounded-md border border-border bg-secondary/30 text-foreground hover:bg-secondary/50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
        >
          −
        </button>
        <span class="w-8 text-center font-semibold">{{ quantity }}</span>
        <button
          @click="() => onUpdateQuantity(coffee.id, quantity + 1)"
          class="flex h-10 w-10 items-center justify-center rounded-md border border-border bg-secondary/30 text-foreground hover:bg-secondary/50 transition-colors"
        >
          +
        </button>
      </div>

      <!-- Add to Cart Button -->
      <button
        v-if="!isSoldOut"
        @click="() => onAddToCart(coffee)"
        class="w-full rounded-lg bg-accent text-accent-foreground font-semibold py-2.5 hover:bg-accent/90 transition-colors duration-200"
      >
        {{ quantity > 0 ? 'Add More' : 'Add to Cart' }}
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CoffeeCard',
  props: {
    coffee: {
      type: Object,
      required: true,
    },
    onAddToCart: {
      type: Function,
      required: true,
    },
    onUpdateQuantity: {
      type: Function,
      required: true,
    },
    cartItem: {
      type: Object,
      default: null,
    },
  },
  computed: {
    isSoldOut() {
      return this.coffee.stock === 0
    },
    quantity() {
      return this.cartItem?.quantity || 0
    },
  },
}
</script>