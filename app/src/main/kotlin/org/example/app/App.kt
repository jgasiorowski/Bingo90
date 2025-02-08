/*
 * This source file was generated by the Gradle 'init' task
 */
package org.example.app

import org.example.core.*
import org.example.core.generation.Generator
import kotlin.time.measureTime


fun main() {
    var strip: Strip? = null
    val timeTaken = measureTime {
        for (i in (1..10000)){
            strip = Generator().Generate()
        }

    }

    println(timeTaken)
    printuj(strip!!)
}

fun printuj(strip: Strip) {
    for (ticket in strip.tickets){
        printuj(ticket)
    }
}

fun printuj(ticket: Ticket) {
    println()
    println("--------------------------")
    for (row in ticket.rows){
        println(row.map { i -> if (i == null)  "  " else String.format("%02d", i) }.joinToString("|"))
    }
    println("--------------------------")
}
