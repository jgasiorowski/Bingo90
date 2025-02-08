package org.example.app

import org.example.core.Strip
import org.example.core.Ticket

class StripPrinter {
    companion object {
        fun print(strip: Strip){
            for (ticket in strip.tickets){
                print(ticket)
            }
        }

        private fun print(ticket: Ticket) {
            println()
            println("--------------------------")
            for (row in ticket.rows){
                println(row.map { i -> if (i == null)  "  " else String.format("%02d", i) }.joinToString("|"))
            }
            println("--------------------------")
        }
    }
}