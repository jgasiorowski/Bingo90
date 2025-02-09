/*
 * This source file was generated by the Gradle 'init' task
 */
package org.example.app

import org.example.core.*
import org.example.core.generation.StripBuildDirector
import kotlin.time.measureTime


fun main() {
    var strip: Strip? = null
    val timeTaken = measureTime {
        for (i in (1..10000)){
            strip = StripBuildDirector().make()
        }
    }

    println(timeTaken)
    StripPrinter.print(strip!!);
}


