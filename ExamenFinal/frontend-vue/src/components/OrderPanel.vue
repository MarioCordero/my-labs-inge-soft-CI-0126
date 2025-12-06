<template>
  <div class="w-full lg:w-96 flex flex-col gap-6">
    <!-- Order Summary -->
    <div class="rounded-lg border border-border bg-card p-6">
      <h3 class="text-lg font-semibold mb-4">Order Summary</h3>

      <div v-if="cart.length === 0" class="text-muted-foreground text-sm text-center py-8">
        No items selected yet
      </div>
      <div v-else>
        <div class="space-y-3 mb-6 max-h-48 overflow-y-auto">
          <div v-for="item in cart" :key="item.id" class="flex items-center justify-between text-sm">
            <div class="flex-1">
              <p class="font-medium">{{ item.name }}</p>
              <p class="text-muted-foreground text-xs">
                ₡{{ item.price.toLocaleString() }} × {{ item.quantity }}
              </p>
            </div>
            <div class="flex items-center gap-2">
              <p class="font-semibold w-16 text-right">₡{{ (item.price * item.quantity).toLocaleString() }}</p>
              <button
                @click="() => onUpdateQuantity(item.id, item.quantity - 1)"
                class="text-xs text-muted-foreground hover:text-foreground"
              >
                ×
              </button>
            </div>
          </div>
        </div>

        <!-- Total Cost -->
        <div class="border-t border-border pt-4">
          <div class="flex justify-between items-center">
            <p class="text-muted-foreground">Total Cost:</p>
            <p class="text-2xl font-bold text-accent">₡{{ totalCost.toLocaleString() }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Payment Input -->
    <div class="rounded-lg border border-border bg-card p-6">
      <h3 class="text-lg font-semibold mb-4">Payment</h3>

      <!-- Payment Buttons -->
      <div class="grid grid-cols-3 gap-2 mb-6">
        <button
          v-for="denom in paymentDenominations"
          :key="denom.value"
          @click="() => onAddPayment(denom.value)"
          :disabled="cart.length === 0"
          class="rounded-lg border border-border bg-secondary/30 px-3 py-2 text-sm font-medium text-foreground hover:bg-secondary/50 hover:border-accent/50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
        >
          <div class="text-xs text-muted-foreground">{{ denom.type }}</div>
          <div class="font-semibold">₡{{ denom.label }}</div>
        </button>
      </div>

      <!-- Paid Amount Display -->
      <div class="bg-secondary/20 rounded-lg p-4 mb-6 border border-border/50">
        <p class="text-sm text-muted-foreground mb-2">Paid Amount:</p>
        <p class="text-3xl font-bold text-accent">₡{{ paidAmount.toLocaleString() }}</p>
        <p v-if="cart.length > 0 && totalCost > 0" class="text-xs text-muted-foreground mt-2">
          {{ paidAmount >= totalCost
            ? `Need: ₡${(totalCost - paidAmount).toLocaleString()}`
            : `Remaining: ₡${(totalCost - paidAmount).toLocaleString()}`
          }}
        </p>
      </div>

      <!-- Pay Button -->
      <button
        @click="onPay"
        :disabled="!canPay"
        :class="[
          'w-full rounded-lg font-bold py-4 text-lg transition-colors duration-200',
          canPay
            ? 'bg-accent text-accent-foreground hover:bg-accent/90 cursor-pointer'
            : 'bg-secondary/30 text-muted-foreground cursor-not-allowed opacity-50'
        ]"
      >
        {{ cart.length === 0 ? 'Select Coffee' : `Pay ₡${totalCost.toLocaleString()}` }}
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'OrderPanel',
  props: {
    cart: {
      type: Array,
      required: true,
    },
    totalCost: {
      type: Number,
      required: true,
    },
    paidAmount: {
      type: Number,
      required: true,
    },
    change: {
      type: Number,
      required: true,
    },
    canPay: {
      type: Boolean,
      required: true,
    },
    paymentDenominations: {
      type: Array,
      required: true,
    },
    onAddPayment: {
      type: Function,
      required: true,
    },
    onPay: {
      type: Function,
      required: true,
    },
    onUpdateQuantity: {
      type: Function,
      required: true,
    },
  },
}
</script>