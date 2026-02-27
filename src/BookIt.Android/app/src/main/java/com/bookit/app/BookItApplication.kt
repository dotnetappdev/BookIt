package com.bookit.app

import android.app.Application

class BookItApplication : Application() {
    override fun onCreate() {
        super.onCreate()
        instance = this
    }

    companion object {
        lateinit var instance: BookItApplication
            private set
    }
}
