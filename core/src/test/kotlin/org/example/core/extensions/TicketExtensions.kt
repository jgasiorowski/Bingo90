package org.example.core.extensions

import org.example.core.Ticket

val Ticket.columns: Array<Array<Int?>>
    get() {
        val columns = Array(9) { arrayOfNulls<Int>(3) }
        for (columnIndex in 0..8) {
            columns[columnIndex] = arrayOf(
                this.rows[0][columnIndex],
                this.rows[1][columnIndex],
                this.rows[2][columnIndex]
            )
        }

        return columns
    }