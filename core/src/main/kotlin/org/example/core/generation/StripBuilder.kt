package org.example.core.generation

import org.example.core.Strip

class StripBuilder() {
    fun build(): Strip {
        return Strip(tickets.map { t -> t.build() }.toTypedArray())
    }

    fun distribute(value: Int, rangeIndex: Int) {
        val min = tickets.map { t -> t.numbersCount }.min()
        val a = mutableListOf<Int>()
        val b = mutableListOf<Int>()

        for (ticket in tickets){
            if (ticket.numbersCount == min) {
                a.add(ticket.index)
            } else{
                b.add(ticket.index)
            }
        }

        a.shuffle()
        val ordered = a + b

        for (i in tickets.indices){
            val ticket = tickets[ordered[i]]

            if (ticket.canAccept(value, rangeIndex)){
                ticket.place(value, rangeIndex)
                return
            }
        }
    }

    val tickets = Array(6) { TicketBuilder(it) }
}