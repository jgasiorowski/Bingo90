package org.example.core.generation

import org.example.core.Strip
import kotlin.random.Random

/**
 * This class provides the [create] function which generates the [Strip] class instance
 */
class StripFactory {
    companion object {

        /**
         * Generates a Bingo 90 Strip
         *
         * @return an instance of [Strip]
         */
        fun create(): Strip {
            val tickets = Array(6) { TicketBuilder(it) }
            val availableNumberGroups = NumberGroupsProvider.provide()

            this.distributeEqually(tickets, availableNumberGroups)
            this.distributeBalanced(tickets, availableNumberGroups)

            return this.createStripFromTicketBuilders(tickets)
        }


        /**
         * This function randomly distributes **one** number per column in each [TicketBuilder].
         *
         * Number gets removed from [availableNumberGroups] after being placed on ticket
         */
        private fun distributeEqually(tickets: Array<TicketBuilder>, availableNumberGroups: Array<MutableList<Int>>) {
            for (ticket in tickets) {
                for (groupIndex in availableNumberGroups.indices) {
                    val randomNumberIndex = Random.nextInt(availableNumberGroups[groupIndex].size)
                    ticket.place(availableNumberGroups[groupIndex][randomNumberIndex], groupIndex)
                    availableNumberGroups[groupIndex].removeAt(randomNumberIndex)
                }
            }
        }

        /**
         * This function is randomly distributing rest of the numbers from [availableNumberGroups] onto [TicketBuilder].
         * This is being done by taking random number from [availableNumberGroups] and trying to place it in ticket
         * which has the most available cells and can receive another number to matching column (maximum 3 numbers per ticket column)
         *
         * Number gets removed from [availableNumberGroups] after being placed on ticket
         */
        private fun distributeBalanced(tickets: Array<TicketBuilder>, availableNumberGroups: Array<MutableList<Int>>) {
            for (groupIndex in availableNumberGroups.indices) {
                val group = availableNumberGroups[groupIndex]
                do {
                    val randomNumberIndex = Random.nextInt(group.size)
                    this.distribute(tickets, group[randomNumberIndex], groupIndex)
                    group.removeAt(randomNumberIndex)
                } while (group.size > 0)
            }
        }

        private fun createStripFromTicketBuilders(tickets: Array<TicketBuilder>): Strip {
            return Strip(tickets.map { ticket -> ticket.build() }.toTypedArray())
        }

        private fun distribute(tickets: Array<TicketBuilder>, value: Int, rangeIndex: Int) {
            val minimalNumbersCountInTickets = tickets.minOf { t -> t.numbersInTicketCount }
            val ticketsWithMinimalNumbersCount = mutableListOf<Int>()
            val otherTickets = mutableListOf<Int>()

            for (ticket in tickets) {
                if (ticket.numbersInTicketCount == minimalNumbersCountInTickets) {
                    ticketsWithMinimalNumbersCount.add(ticket.index)
                } else {
                    otherTickets.add(ticket.index)
                }
            }

            ticketsWithMinimalNumbersCount.shuffle()
            val ordered = ticketsWithMinimalNumbersCount + otherTickets

            for (i in tickets.indices) {
                val ticket = tickets[ordered[i]]

                if (ticket.hasSpace(rangeIndex)) {
                    ticket.place(value, rangeIndex)
                    return
                }
            }
        }
    }
}